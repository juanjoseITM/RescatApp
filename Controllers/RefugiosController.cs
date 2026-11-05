using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RescatApp.Models;
using RescatApp.Services;

namespace RescatApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefugiosController : ControllerBase
    {
        private readonly RefugioService _refugioService;

        public RefugiosController(RefugioService refugioService)
        {
            _refugioService = refugioService;
        }

        [HttpGet]
        public ActionResult<List<Refugio>> Get()
        {
            return Ok(_refugioService.ListarRefugios());
        }

        [HttpPost]
        public IActionResult Post([FromBody] Refugio refugio)
        {
            _refugioService.CrearRefugio(refugio);
            return Ok(new { mensaje = "Refugio registrado con éxito" });
        }
    }
}
