using Microsoft.AspNetCore.Mvc;
using SolutionApi.DTOs;
using SolutionApi.Models;
using SolutionApi.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace SolutionApi.Controllers
{
    /// <summary>
    /// Controller para gerenciar as pessoas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Pessoas")]
    public class PessoaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor do controller de pessoas.
        /// </summary>
        /// <param name="context">Contexto de acesso ao banco de dados.</param>
        public PessoaController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todas as pessoas cadastradas.
        /// </summary>
        /// <returns>Uma lista de pessoas ou uma resposta de 'No Content' se não houver pessoas.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Listar todas as pessoas", Description = "Este endpoint retorna todas as pessoas cadastradas.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas()
        {
            var pessoas = await _context.Pessoas.ToListAsync();

            if (!pessoas.Any())
            {
                return NoContent(); // Retorna caso não haja pessoas cadastradas
            }

            // Resposta limpa com as informações relevantes
            var response = pessoas.Select(p => new
            {
                message = $"Pessoa {p.Nome} encontrada com sucesso.",
                data = new
                {
                    p.Nome,
                    p.Idade,
                    p.Bairro,
                    p.PCD,
                    p.CPF,
                    p.Carreira
                }
            }).ToList();

            return Ok(response); // Retorna a lista de pessoas com a mensagem de sucesso
        }

        /// <summary>
        /// Retorna uma pessoa pelo CPF.
        /// </summary>
        /// <param name="cpf">O CPF da pessoa.</param>
        /// <returns>A pessoa encontrada ou uma resposta de 'Not Found' se não encontrar.</returns>
        [HttpGet("{cpf}")]
        [SwaggerOperation(Summary = "Buscar pessoa por CPF", Description = "Este endpoint busca uma pessoa através do CPF.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<Pessoa>> GetPessoa(string cpf)
        {
            var pessoa = await _context.Pessoas.FindAsync(cpf);

            if (pessoa == null)
            {
                return NotFound(new { message = "Pessoa não encontrada com o CPF fornecido." });
            }

            var response = new
            {
                message = $"Pessoa {pessoa.Nome} encontrada com sucesso.",
                data = new
                {
                    pessoa.Nome,
                    pessoa.Idade,
                    pessoa.Bairro,
                    pessoa.PCD,
                    pessoa.CPF,
                    pessoa.Carreira
                }
            };

            return Ok(response); // Retorna a pessoa encontrada com a mensagem de sucesso
        }

        /// <summary>
        /// Cadastra uma nova pessoa.
        /// </summary>
        /// <param name="pessoaDto">Os dados da pessoa a ser cadastrada.</param>
        /// <returns>Resposta de sucesso com a pessoa criada ou erro de validação.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Cadastrar nova pessoa", Description = "Este endpoint cria uma nova pessoa.")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
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

            return CreatedAtAction(nameof(GetPessoa), new { cpf = pessoa.CPF }, new { message = "Pessoa criada com sucesso.", data = pessoa });
        }

        /// <summary>
        /// Atualiza os dados de uma pessoa.
        /// </summary>
        /// <param name="cpf">O CPF da pessoa a ser atualizada.</param>
        /// <param name="pessoaDto">Os novos dados da pessoa.</param>
        /// <returns>Resposta de sucesso ou erro de validação.</returns>
        [HttpPut("{cpf}")]
        [SwaggerOperation(Summary = "Atualizar dados de uma pessoa", Description = "Este endpoint permite atualizar os dados de uma pessoa associada ao CPF.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> PutPessoa(string cpf, [FromBody] PessoaDto pessoaDto)
        {
            var pessoa = await _context.Pessoas.FindAsync(cpf);

            if (pessoa == null)
            {
                return NotFound(new { message = "Pessoa não encontrada para atualização." });
            }

            pessoa.Nome = pessoaDto.Nome;
            pessoa.Idade = pessoaDto.Idade;
            pessoa.Bairro = pessoaDto.Bairro;
            pessoa.PCD = pessoaDto.PCD;
            pessoa.Senha = pessoaDto.Senha;
            pessoa.Carreira = pessoaDto.Carreira;

            _context.Entry(pessoa).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent(); // Confirmação de sucesso sem conteúdo
        }

        /// <summary>
        /// Deleta uma pessoa.
        /// </summary>
        /// <param name="cpf">O CPF da pessoa a ser excluída.</param>
        /// <returns>Resposta de sucesso ou erro se não encontrar a pessoa.</returns>
        [HttpDelete("{cpf}")]
        [SwaggerOperation(Summary = "Deletar uma pessoa", Description = "Este endpoint exclui uma pessoa do sistema com base no CPF.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> DeletePessoa(string cpf)
        {
            var pessoa = await _context.Pessoas.FindAsync(cpf);

            if (pessoa == null)
            {
                return NotFound(new { message = "Pessoa não encontrada para exclusão." });
            }

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();

            return NoContent(); // Confirmação de exclusão sem conteúdo
        }
    }
}
