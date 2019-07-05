using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SisbolApi.Models
{
    public class LogDbContext : DbContext
    {
        public DbSet<ApiLogItem> ApiLogItem { get; set; }

        public LogDbContext(DbContextOptions<LogDbContext> options) : base(options)
        {
        }
    }
}