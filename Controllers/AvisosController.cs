using Microsoft.AspNetCore.Mvc;
using SolutionApi.DTOs;
using SolutionApi.Models;
using SolutionApi.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace SolutionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "Avisos")]  // Agrupando no grupo "Avisos" no Swagger
    [Tags("Avisos")]   //Para categorizar os endpoints no Swagger
    public class AvisoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AvisoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Avisos
        [HttpGet]
        [SwaggerOperation(Summary = "Retorna todos os avisos", Description = "Retorna uma lista com todos os avisos registrados.")]
        [ProducesResponseType(typeof(IEnumerable<Aviso>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAvisos()
        {
            var avisos = await _context.Avisos.ToListAsync();
            if (avisos == null || !avisos.Any())
            {
                return NoContent();
            }

            return Ok(avisos);
        }

        // GET: api/Avisos/{tipoAviso}
        [HttpGet("{tipoAviso}")]
        [SwaggerOperation(Summary = "Retorna um aviso pelo tipo", Description = "Retorna um aviso baseado no tipo do evento climático.")]
        [ProducesResponseType(typeof(Aviso), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAviso(string tipoAviso)
        {
            var aviso = await _context.Avisos.FirstOrDefaultAsync(a => a.TipoAviso == tipoAviso);

            if (aviso == null)
            {
                return NotFound(new { message = "Aviso não encontrado." });
            }

            return Ok(aviso);
        }

        // POST: api/Avisos
        [HttpPost]
        [SwaggerOperation(Summary = "Cria um novo aviso", Description = "Cria um novo aviso com as informações fornecidas.")]
        [ProducesResponseType(typeof(Aviso), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAviso([FromBody] AvisoDto avisoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos.", errors = ModelState });
            }

            var aviso = new Aviso
            {
                TipoAviso = avisoDto.TipoAviso,
                Ocorrencia = avisoDto.Ocorrencia,
                Gravidade = avisoDto.Gravidade,
                Bairro = avisoDto.Bairro
            };

            _context.Avisos.Add(aviso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAviso), new { tipoAviso = aviso.TipoAviso }, aviso);
        }

        // PUT: api/Avisos/{tipoAviso}
        [HttpPut("{tipoAviso}")]
        [SwaggerOperation(Summary = "Atualiza um aviso existente", Description = "Atualiza as informações de um aviso baseado no tipo do evento climático.")]
        [ProducesResponseType(typeof(Aviso), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAviso(string tipoAviso, [FromBody] AvisoDto avisoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos.", errors = ModelState });
            }

            var aviso = await _context.Avisos.FirstOrDefaultAsync(a => a.TipoAviso == tipoAviso);

            if (aviso == null)
            {
                return NotFound(new { message = "Aviso não encontrado." });
            }

            aviso.Ocorrencia = avisoDto.Ocorrencia;
            aviso.Gravidade = avisoDto.Gravidade;
            aviso.Bairro = avisoDto.Bairro;

            _context.Avisos.Update(aviso);
            await _context.SaveChangesAsync();

            return Ok(aviso);
        }

        // DELETE: api/Avisos/{tipoAviso}
        [HttpDelete("{tipoAviso}")]
        [SwaggerOperation(Summary = "Deleta um aviso", Description = "Deleta um aviso existente com base no tipo do evento climático.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAviso(string tipoAviso)
        {
            var aviso = await _context.Avisos.FirstOrDefaultAsync(a => a.TipoAviso == tipoAviso);

            if (aviso == null)
            {
                return NotFound(new { message = "Aviso não encontrado." });
            }

            _context.Avisos.Remove(aviso);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
