using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using SisbolApi.Models;
using SisbolApi.Security;

/*
Controller para gerenciar a geracao de token para as requisicoes. 
Necessario implementar as validacoes de usuario e senha como for melhor para o CB.
*/

namespace SisbolApi.Controllers
{
    [Route("api/login")]
    [AllowAnonymous]
    public class AuthenticationController : Controller
    {
        [HttpPost]
        public IActionResult Create([FromBody]LoginInputModel inputModel)
        {
            //Substituir pela validacao no banco de dados ou outra
            if (inputModel.Username == "seccom" && inputModel.Password == "seccom")
            {
                var token = new JwtTokenBuilder()
                                    .AddSecurityKey(JwtSecurityKey.Create("senhasecretasisbolapi"))
                                    .AddSubject("Aplicacao Seccom")
                                    .AddIssuer("Sisbol.Security.Bearer")
                                    .AddAudience("Sisbol.Security.Bearer")
                                    .AddClaim("InspecaoSaude", "true")
                                    .AddClaim("Punicoes", "true")
                                    .AddClaim("Elogios", "true")
                                    .AddExpiry(10)
                                    .Build();
                return Ok(token.Value);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}