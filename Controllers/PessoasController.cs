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
    [Tags("Pessoas")]
    public class PessoaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PessoaController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Pessoas
        [HttpGet]
        [EndpointSummary("Listar todas as pessoas")]
        [EndpointDescription("Este endpoint retorna todas as pessoas cadastradas.")]
        [ProducesResponseType(typeof(IEnumerable<PessoaDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPessoas()
        {
            var pessoas = await _context.Pessoas.ToListAsync();
            if (pessoas == null || !pessoas.Any())
            {
                return NotFound(new { Message = "Nenhuma pessoa encontrada." });
            }
            return Ok(pessoas);
        }

        // GET: api/Pessoas/5
        [HttpGet("{cpf}")]
        [EndpointSummary("Buscar pessoa por CPF")]
        [EndpointDescription("Este endpoint busca uma pessoa através do CPF.")]
        [ProducesResponseType(typeof(PessoaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPessoa(string cpf)
        {
            var pessoa = await _context.Pessoas.FindAsync(cpf);

            if (pessoa == null)
            {
                return NotFound(new { Message = "Pessoa não encontrada." });
            }

            return Ok(pessoa);
        }

        // POST: api/Pessoas
        [HttpPost]
        [EndpointSummary("Cadastrar nova pessoa")]
        [EndpointDescription("Este endpoint cria uma nova pessoa.")]
        [ProducesResponseType(typeof(PessoaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostPessoa([FromBody] PessoaDto pessoaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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

        // PUT: api/Pessoas/5
        [HttpPut("{cpf}")]
        [EndpointSummary("Atualizar dados de uma pessoa")]
        [EndpointDescription("Este endpoint permite atualizar os dados de uma pessoa.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutPessoa(string cpf, [FromBody] PessoaDto pessoaDto)
        {
            var pessoa = await _context.Pessoas.FindAsync(cpf);
            if (pessoa == null)
            {
                return NotFound(new { Message = "Pessoa não encontrada." });
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

        // DELETE: api/Pessoas/5
        [HttpDelete("{cpf}")]
        [EndpointSummary("Deletar uma pessoa")]
        [EndpointDescription("Este endpoint exclui uma pessoa do sistema.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletePessoa(string cpf)
        {
            var pessoa = await _context.Pessoas.FindAsync(cpf);
            if (pessoa == null)
            {
                return NotFound(new { Message = "Pessoa não encontrada." });
            }

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
