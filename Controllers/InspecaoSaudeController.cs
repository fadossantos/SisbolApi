using Microsoft.AspNetCore.Mvc;
using SisbolApi.Models;
using Microsoft.AspNetCore.Authorization;

namespace SisbolApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InspecaoSaudeController : ControllerBase
    {
        private readonly BDContext _context;

        public InspecaoSaudeController(BDContext context)
        {
            _context = context;
        }

        // GET: api/inspecaosaude/{re}
        // Necessita da ROLE InspecaoSaude no token para acessar esse metodo.
        [Authorize(Policy = "InspecaoSaude")]
        [HttpGet("{re}")]
        public ActionResult<InspecaoSaude> GetInspecaoSaude(string re)
        {
            var insp = _context.buscarInspecao(re);
            if (insp == null)
            {
                return NotFound();
            }
            return insp;
        }
    }
}