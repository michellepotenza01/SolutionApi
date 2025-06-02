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
        [SwaggerOperation(Summary = "Listar todos os voluntários", Description = "Este endpoint retorna todos os voluntários cadastrados.")]
        [ProducesResponseType(typeof(IEnumerable<VoluntarioDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetVoluntarios()
        {
            var voluntarios = await _context.Voluntarios
                .Include(v => v.Pessoa)  // Inclui as informações da Pessoa associada
                .Include(v => v.Abrigo)  // Inclui as informações do Abrigo associado
                .ToListAsync();

            if (voluntarios == null || !voluntarios.Any())
            {
                return NoContent();  // Caso não haja voluntários, retorna NoContent
            }

            return Ok(voluntarios);
        }

        // GET: api/Voluntarios/{cpf}
        [HttpGet("{cpf}")]
        [SwaggerOperation(Summary = "Buscar voluntário por CPF", Description = "Este endpoint busca um voluntário através do CPF.")]
        [ProducesResponseType(typeof(VoluntarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVoluntario(string cpf)
        {
            var voluntario = await _context.Voluntarios
                .Include(v => v.Pessoa)
                .Include(v => v.Abrigo)
                .FirstOrDefaultAsync(v => v.CPF == cpf);

            if (voluntario == null)
            {
                return NotFound(new { message = "Voluntário não encontrado." });
            }

            return Ok(voluntario);
        }

        // POST: api/Voluntarios
        [HttpPost]
        [SwaggerOperation(Summary = "Cadastrar novo voluntário", Description = "Este endpoint cria um novo voluntário. Deve associar o CPF de uma pessoa existente.")]
        [ProducesResponseType(typeof(VoluntarioDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostVoluntario([FromBody] VoluntarioDto voluntarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos.", errors = ModelState });
            }

            var pessoa = await _context.Pessoas.FindAsync(voluntarioDto.CPF);
            if (pessoa == null)
            {
                return BadRequest(new { message = "CPF não encontrado para associar ao voluntário." });
            }

            var voluntario = new Voluntario
            {
                RG = voluntarioDto.RG,
                CPF = voluntarioDto.CPF,
                Funcao = voluntarioDto.Funcao,
                NomeAbrigo = voluntarioDto.NomeAbrigo,
                Pessoa = pessoa  // Associa a pessoa com o voluntário
            };

            _context.Voluntarios.Add(voluntario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVoluntario), new { cpf = voluntario.CPF }, voluntario);
        }

        // PUT: api/Voluntarios/{cpf}
        [HttpPut("{cpf}")]
        [SwaggerOperation(Summary = "Atualizar dados de um voluntário", Description = "Este endpoint permite atualizar os dados de um voluntário associado ao CPF.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutVoluntario(string cpf, [FromBody] VoluntarioDto voluntarioDto)
        {
            var voluntario = await _context.Voluntarios.FindAsync(cpf);
            if (voluntario == null)
            {
                return NotFound(new { message = "Voluntário não encontrado." });
            }

            voluntario.RG = voluntarioDto.RG;
            voluntario.Funcao = voluntarioDto.Funcao;
            voluntario.NomeAbrigo = voluntarioDto.NomeAbrigo;

            _context.Entry(voluntario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Voluntarios/{cpf}
        [HttpDelete("{cpf}")]
        [SwaggerOperation(Summary = "Deletar um voluntário", Description = "Este endpoint exclui um voluntário do sistema com base no CPF.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVoluntario(string cpf)
        {
            var voluntario = await _context.Voluntarios.FindAsync(cpf);
            if (voluntario == null)
            {
                return NotFound(new { message = "Voluntário não encontrado." });
            }

            _context.Voluntarios.Remove(voluntario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
