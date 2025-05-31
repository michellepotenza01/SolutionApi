using Microsoft.AspNetCore.Mvc;
using SolutionApi.DTOs;
using SolutionApi.Models;
using SolutionApi.Data;
using Microsoft.EntityFrameworkCore;
using SolutionApi.Enums;
using System.Net;

namespace SolutionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Avisos")]
    public class AvisoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AvisoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/aviso
        [HttpGet]
        [EndpointSummary("Retorna todos os avisos.")]
        [EndpointDescription("Retorna uma lista com todos os avisos registrados.")]
        [ProducesResponseType(typeof(IEnumerable<Aviso>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetAvisos()
        {
            var avisos = await _context.Avisos.ToListAsync();

            if (avisos == null || !avisos.Any())
            {
                return NoContent();
            }

            return Ok(avisos);
        }

        // GET: api/aviso/{tipoAviso}
        [HttpGet("{tipoAviso}")]
        [EndpointSummary("Retorna um aviso pelo tipo.")]
        [EndpointDescription("Retorna um aviso baseado no tipo do evento climático.")]
        [ProducesResponseType(typeof(Aviso), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAviso(string tipoAviso)
        {
            var aviso = await _context.Avisos
                .FirstOrDefaultAsync(a => a.TipoAviso == tipoAviso);

            if (aviso == null)
            {
                return NotFound(new { message = "Aviso não encontrado." });
            }

            return Ok(aviso);
        }

        // POST: api/aviso
        [HttpPost]
        [EndpointSummary("Cria um novo aviso.")]
        [EndpointDescription("Cria um novo aviso com as informações fornecidas.")]
        [ProducesResponseType(typeof(Aviso), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
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

        // PUT: api/aviso/{tipoAviso}
        [HttpPut("{tipoAviso}")]
        [EndpointSummary("Atualiza um aviso existente.")]
        [EndpointDescription("Atualiza as informações de um aviso baseado no tipo do evento climático.")]
        [ProducesResponseType(typeof(Aviso), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateAviso(string tipoAviso, [FromBody] AvisoDto avisoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos.", errors = ModelState });
            }

            var aviso = await _context.Avisos
                .FirstOrDefaultAsync(a => a.TipoAviso == tipoAviso);

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

        // DELETE: api/aviso/{tipoAviso}
        [HttpDelete("{tipoAviso}")]
        [EndpointSummary("Deleta um aviso.")]
        [EndpointDescription("Deleta um aviso existente com base no tipo do evento climático.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteAviso(string tipoAviso)
        {
            var aviso = await _context.Avisos
                .FirstOrDefaultAsync(a => a.TipoAviso == tipoAviso);

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
