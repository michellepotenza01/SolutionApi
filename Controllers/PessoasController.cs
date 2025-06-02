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
    //[ApiExplorerSettings(GroupName = "Pessoas")] // Agrupando os endpoints de "Pessoas"
    public class PessoaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PessoaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Pessoas
        [HttpGet]
        [SwaggerOperation(Summary = "Listar todas as pessoas", Description = "Este endpoint retorna todas as pessoas cadastradas.")]
        [ProducesResponseType(typeof(IEnumerable<PessoaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetPessoas()
        {
            var pessoas = await _context.Pessoas.ToListAsync();
            if (pessoas == null || !pessoas.Any())
            {
                return NoContent();
            }
            return Ok(pessoas);
        }

        // GET: api/Pessoas/{cpf}
        [HttpGet("{cpf}")]
        [SwaggerOperation(Summary = "Buscar pessoa por CPF", Description = "Este endpoint busca uma pessoa através do CPF.")]
        [ProducesResponseType(typeof(PessoaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPessoa(string cpf)
        {
            var pessoa = await _context.Pessoas.FindAsync(cpf);

            if (pessoa == null)
            {
                return NotFound(new { message = "Pessoa não encontrada." });
            }

            return Ok(pessoa);
        }

        // POST: api/Pessoas
        [HttpPost]
        [SwaggerOperation(Summary = "Cadastrar nova pessoa", Description = "Este endpoint cria uma nova pessoa.")]
        [ProducesResponseType(typeof(PessoaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostPessoa([FromBody] PessoaDto pessoaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos.", errors = ModelState });
            }

            var pessoa = new Pessoa
            {
                Nome = pessoaDto.Nome,
                Idade = pessoaDto.Idade,
                Bairro = pessoaDto.Bairro,
                PCD = pessoaDto.PCD,
                Senha = pessoaDto.Senha,
                CPF = pessoaDto.CPF,
                Carreira = pessoaDto.Carreira
            };

            _context.Pessoas.Add(pessoa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPessoa), new { cpf = pessoa.CPF }, pessoa);
        }

        // PUT: api/Pessoas/{cpf}
        [HttpPut("{cpf}")]
        [SwaggerOperation(Summary = "Atualizar dados de uma pessoa", Description = "Este endpoint permite atualizar os dados de uma pessoa.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutPessoa(string cpf, [FromBody] PessoaDto pessoaDto)
        {
            var pessoa = await _context.Pessoas.FindAsync(cpf);
            if (pessoa == null)
            {
                return NotFound(new { message = "Pessoa não encontrada." });
            }

            pessoa.Nome = pessoaDto.Nome;
            pessoa.Idade = pessoaDto.Idade;
            pessoa.Bairro = pessoaDto.Bairro;
            pessoa.PCD = pessoaDto.PCD;
            pessoa.Senha = pessoaDto.Senha;
            pessoa.Carreira = pessoaDto.Carreira;

            _context.Entry(pessoa).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Pessoas/{cpf}
        [HttpDelete("{cpf}")]
        [SwaggerOperation(Summary = "Deletar uma pessoa", Description = "Este endpoint exclui uma pessoa do sistema com base no CPF.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePessoa(string cpf)
        {
            var pessoa = await _context.Pessoas.FindAsync(cpf);
            if (pessoa == null)
            {
                return NotFound(new { message = "Pessoa não encontrada." });
            }

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
