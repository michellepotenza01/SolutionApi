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
    [Tags("Voluntários")]
    public class VoluntarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VoluntarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Voluntarios
        [HttpGet]
        [EndpointSummary("Listar todos os voluntários")]
        [EndpointDescription("Este endpoint retorna todos os voluntários cadastrados.")]
        [ProducesResponseType(typeof(IEnumerable<VoluntarioDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetVoluntarios()
        {
            var voluntarios = await _context.Voluntarios.Include(v => v.Pessoa).Include(v => v.Abrigo).ToListAsync();
            if (voluntarios == null || !voluntarios.Any())
            {
                return NotFound(new { Message = "Nenhum voluntário encontrado." });
            }
            return Ok(voluntarios);
        }

        // GET: api/Voluntarios/5
        [HttpGet("{cpf}")]
        [EndpointSummary("Buscar voluntário por CPF")]
        [EndpointDescription("Este endpoint busca um voluntário através do CPF.")]
        [ProducesResponseType(typeof(VoluntarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVoluntario(string cpf)
        {
            var voluntario = await _context.Voluntarios.Include(v => v.Pessoa).Include(v => v.Abrigo).FirstOrDefaultAsync(v => v.CPF == cpf);

            if (voluntario == null)
            {
                return NotFound(new { Message = "Voluntário não encontrado." });
            }

            return Ok(voluntario);
        }

        // POST: api/Voluntarios
        [HttpPost]
        [EndpointSummary("Cadastrar novo voluntário")]
        [EndpointDescription("Este endpoint cria um novo voluntário.")]
        [ProducesResponseType(typeof(VoluntarioDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostVoluntario([FromBody] VoluntarioDto voluntarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pessoa = await _context.Pessoas.FindAsync(voluntarioDto.CPF);
            if (pessoa == null)
            {
                return BadRequest(new { Message = "CPF não encontrado para associar ao voluntário." });
            }

            var voluntario = new Voluntario
            {
                CPF = voluntarioDto.CPF,
                Funcao = voluntarioDto.Funcao,
                NomeAbrigo = voluntarioDto.NomeAbrigo
            };

            _context.Voluntarios.Add(voluntario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVoluntario), new { cpf = voluntario.CPF }, voluntario);
        }

        // PUT: api/Voluntarios/5
        [HttpPut("{cpf}")]
        [EndpointSummary("Atualizar dados de um voluntário")]
        [EndpointDescription("Este endpoint permite atualizar os dados de um voluntário.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutVoluntario(string cpf, [FromBody] VoluntarioDto voluntarioDto)
        {
            var voluntario = await _context.Voluntarios.FindAsync(cpf);
            if (voluntario == null)
            {
                return NotFound(new { Message = "Voluntário não encontrado." });
            }

            voluntario.Funcao = voluntarioDto.Funcao;
            voluntario.NomeAbrigo = voluntarioDto.NomeAbrigo;

            _context.Entry(voluntario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Voluntarios/5
        [HttpDelete("{cpf}")]
        [EndpointSummary("Deletar um voluntário")]
        [EndpointDescription("Este endpoint exclui um voluntário do sistema.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVoluntario(string cpf)
        {
            var voluntario = await _context.Voluntarios.FindAsync(cpf);
            if (voluntario == null)
            {
                return NotFound(new { Message = "Voluntário não encontrado." });
            }

            _context.Voluntarios.Remove(voluntario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
