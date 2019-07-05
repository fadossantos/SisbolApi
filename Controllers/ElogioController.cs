using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SisbolApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace SisbolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElogioController : ControllerBase
    {
        private readonly BDContext _context;

        public ElogioController(BDContext context)
        {
            _context = context;
        }

        // GET: api/elogio/{re}
        // Necessita da ROLE Elogios no token para acessar esse metodo.
        [Authorize(Policy = "Elogios")]
        [HttpGet("{re}")]
        public ActionResult<List<Elogio>> GetElogios(string re)
        {
            var elogio = _context.buscarElogios(re);
            if (elogio == null)
            {
                return NotFound();
            }
            return elogio;
        }
    }
}