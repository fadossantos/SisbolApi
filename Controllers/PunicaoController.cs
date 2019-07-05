using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SisbolApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace SisbolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PunicaoController : ControllerBase
    {
        private readonly BDContext _context;

        public PunicaoController(BDContext context)
        {
            _context = context;
        }

        // GET: api/punicao/{re}
        // Necessita da ROLE Punicoes no token para acessar esse metodo.
        [Authorize(Policy = "Punicoes")]
        [HttpGet("{re}")]
        public ActionResult<List<Punicao>> GetPunicoes(string re)
        {
            var pun = _context.buscarPunicoes(re);
            if (pun == null)
            {
                return NotFound();
            }
            return pun;
        }
    }
}