
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SisbolApi.Middleware;
using SisbolApi.Models;
using SisbolApi.Security;
using SisbolApi.Services;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace SisbolApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                  options.TokenValidationParameters =
                       new TokenValidationParameters
                       {
                           ValidateIssuer = true,
                           ValidateAudience = true,
                           ValidateLifetime = true,
                           ValidateIssuerSigningKey = true,
                           ValidIssuer = "Sisbol.Security.Bearer",
                           ValidAudience = "Sisbol.Security.Bearer",
                           IssuerSigningKey = JwtSecurityKey.Create("senhasecretasisbolapi")
                       };
              });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("InspecaoSaude",
                    policy => policy.RequireClaim("InspecaoSaude"));
                options.AddPolicy("Punicoes",
                    policy => policy.RequireClaim("Punicoes"));

                options.AddPolicy("Elogios",
                    policy => policy.RequireClaim("Elogios"));
            });

            var connectionString = Configuration.GetConnectionString("BDContext");
            services.AddDbContext<LogDbContext>(options => options.UseSqlServer(connectionString));
            services.AddTransient<ApiLogService>();

            services.AddScoped<BDContext>(provider => new BDContext(Configuration));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Sisbol API", Version = "v1" });
                OpenApiSecurityScheme scheme = new OpenApiSecurityScheme();
                scheme.In = ParameterLocation.Header;
                scheme.Name = "Authorization";
                scheme.Type = SecuritySchemeType.Http;
                scheme.Scheme = "bearer";
                scheme.BearerFormat = "JWT";
                scheme.Description = "Token de autorização obtido no login.";
                c.AddSecurityDefinition("Bearer", scheme);    
                c.AddSecurityRequirement( new OpenApiSecurityRequirement { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] { } } } );            
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, LogDbContext logDbContext)
        {
            logDbContext.Database.EnsureCreated();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMiddleware<ApiLoggingMiddleware>();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("./swagger/v1/swagger.json", "V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}
