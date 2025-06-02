using Microsoft.AspNetCore.Mvc;
using SolutionApi.DTOs;
using SolutionApi.Models;
using SolutionApi.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace SolutionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ApiExplorerSettings(GroupName = "Abrigos")] // Agrupando este controller no grupo "Abrigos"
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
        [SwaggerOperation(Summary = "Retorna todos os abrigos", Description = "Este endpoint retorna uma lista com todos os abrigos registrados.")]
        [ProducesResponseType(typeof(IEnumerable<AbrigoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAbrigos()
        {
            var abrigos = await _context.Abrigos.ToListAsync();

            if (abrigos == null || !abrigos.Any())
            {
                return NoContent();  // Caso não haja abrigos, retorna NoContent
            }

            return Ok(abrigos);  // Retorna todos os abrigos
        }

        // GET: api/abrigos/{nomeAbrigo}
        [HttpGet("{nomeAbrigo}")]
        [SwaggerOperation(Summary = "Retorna um abrigo pelo nome.", Description = "Este endpoint retorna um abrigo baseado no nome.")]
        [ProducesResponseType(typeof(Abrigo), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAbrigo(string nomeAbrigo)
        {
            var abrigo = await _context.Abrigos
                .FirstOrDefaultAsync(a => a.NomeAbrigo == nomeAbrigo);

            if (abrigo == null)
            {
                return NotFound(new { message = "Abrigo não encontrado." });
            }

            return Ok(abrigo); // Retorna o abrigo encontrado
        }

        // POST: api/abrigos
        [HttpPost]
        [SwaggerOperation(Summary = "Cria um novo abrigo.", Description = "Este endpoint cria um novo abrigo com as informações fornecidas.")]
        [ProducesResponseType(typeof(AbrigoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAbrigo([FromBody] AbrigoDto abrigoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos.", errors = ModelState });
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
        [SwaggerOperation(Summary = "Atualiza um abrigo existente.", Description = "Este endpoint atualiza as informações de um abrigo com base no nome.")]
        [ProducesResponseType(typeof(Abrigo), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAbrigo(string nomeAbrigo, [FromBody] AbrigoDto abrigoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos.", errors = ModelState });
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
        [SwaggerOperation(Summary = "Deleta um abrigo.", Description = "Este endpoint deleta um abrigo existente com base no nome.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        // GET: api/abrigos/{nomeAbrigo}/voluntarios
        [HttpGet("{nomeAbrigo}/voluntarios")]
        [SwaggerOperation(Summary = "Listar voluntários associados a um abrigo", Description = "Este endpoint retorna todos os voluntários associados a um abrigo pelo nome.")]
        [ProducesResponseType(typeof(IEnumerable<VoluntarioDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVoluntariosByAbrigo(string nomeAbrigo)
        {
            var abrigo = await _context.Abrigos
                .FirstOrDefaultAsync(a => a.NomeAbrigo == nomeAbrigo);

            if (abrigo == null)
            {
                return NotFound(new { message = "Abrigo não encontrado." });
            }

            var voluntarios = await _context.Voluntarios
                .Where(v => v.NomeAbrigo == nomeAbrigo)
                .ToListAsync();

            return Ok(voluntarios);  // Retorna todos os voluntários associados ao abrigo
        }
    }
}
