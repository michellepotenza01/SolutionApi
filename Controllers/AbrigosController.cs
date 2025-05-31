using Microsoft.AspNetCore.Mvc;
using SolutionApi.DTOs;
using SolutionApi.Models;
using SolutionApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace SolutionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Abrigos")]
    public class AbrigoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AbrigoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/abrigos
        [HttpGet]
        [EndpointSummary("Retorna todos os abrigos.")]
        [EndpointDescription("Retorna uma lista com todos os abrigos registrados.")]
        [ProducesResponseType(typeof(IEnumerable<Abrigo>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> GetAbrigos()
        {
            var abrigos = await _context.Abrigos.ToListAsync();

            if (abrigos == null || !abrigos.Any())
            {
                return NoContent();
            }

            return Ok(abrigos);
        }

        // GET: api/abrigos/{nomeAbrigo}
        [HttpGet("{nomeAbrigo}")]
        [EndpointSummary("Retorna um abrigo pelo nome.")]
        [EndpointDescription("Retorna um abrigo baseado no nome.")]
        [ProducesResponseType(typeof(Abrigo), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAbrigo(string nomeAbrigo)
        {
            var abrigo = await _context.Abrigos
                .FirstOrDefaultAsync(a => a.NomeAbrigo == nomeAbrigo);

            if (abrigo == null)
            {
                return NotFound(new { message = "Abrigo não encontrado." });
            }

            return Ok(abrigo);
        }

        // POST: api/abrigos
        [HttpPost]
        [EndpointSummary("Cria um novo abrigo.")]
        [EndpointDescription("Cria um novo abrigo com as informações fornecidas.")]
        [ProducesResponseType(typeof(Abrigo), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAbrigo([FromBody] AbrigoDto abrigoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var abrigo = new Abrigo
            {
                NomeAbrigo = abrigoDto.NomeAbrigo,
                Bairro = abrigoDto.Bairro,
                Tamanho = abrigoDto.Tamanho
            };

            _context.Abrigos.Add(abrigo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAbrigo), new { nomeAbrigo = abrigo.NomeAbrigo }, abrigo);
        }

        // PUT: api/abrigos/{nomeAbrigo}
        [HttpPut("{nomeAbrigo}")]
        [EndpointSummary("Atualiza um abrigo existente.")]
        [EndpointDescription("Atualiza as informações de um abrigo baseado no nome.")]
        [ProducesResponseType(typeof(Abrigo), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateAbrigo(string nomeAbrigo, [FromBody] AbrigoDto abrigoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var abrigo = await _context.Abrigos
                .FirstOrDefaultAsync(a => a.NomeAbrigo == nomeAbrigo);

            if (abrigo == null)
            {
                return NotFound(new { message = "Abrigo não encontrado." });
            }

            abrigo.Bairro = abrigoDto.Bairro;
            abrigo.Tamanho = abrigoDto.Tamanho;

            _context.Abrigos.Update(abrigo);
            await _context.SaveChangesAsync();

            return Ok(abrigo);
        }

        // DELETE: api/abrigos/{nomeAbrigo}
        [HttpDelete("{nomeAbrigo}")]
        [EndpointSummary("Deleta um abrigo.")]
        [EndpointDescription("Deleta um abrigo existente com base no nome.")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteAbrigo(string nomeAbrigo)
        {
            var abrigo = await _context.Abrigos
                .FirstOrDefaultAsync(a => a.NomeAbrigo == nomeAbrigo);

            if (abrigo == null)
            {
                return NotFound(new { message = "Abrigo não encontrado." });
            }

            _context.Abrigos.Remove(abrigo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
