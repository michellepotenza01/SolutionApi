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
    //[ApiExplorerSettings(GroupName = "Pessoas")]
    //[Tags("Pessoas")]
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
        [ProducesResponseType(typeof(IEnumerable<PessoaDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Pessoa>>> GetPessoas()
        {
            var pessoas = await _context.Pessoas.ToListAsync();

            if (!pessoas.Any())
            {
                return NoContent(); // Retorno caso não haja pessoas cadastradas
            }

            return Ok(pessoas); // Retorna a lista de pessoas
        }

        /// <summary>
        /// Retorna uma pessoa pelo CPF.
        /// </summary>
        /// <param name="cpf">O CPF da pessoa.</param>
        /// <returns>A pessoa encontrada ou uma resposta de 'Not Found' se não encontrar.</returns>
        [HttpGet("{cpf}")]
        [SwaggerOperation(Summary = "Buscar pessoa por CPF", Description = "Este endpoint busca uma pessoa através do CPF.")]
        [ProducesResponseType(typeof(PessoaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<Pessoa>> GetPessoa(string cpf)
        {
            var pessoa = await _context.Pessoas.FindAsync(cpf);

            if (pessoa == null)
            {
                return NotFound(new { message = "Pessoa não encontrada com o CPF fornecido." });
            }

            return Ok(pessoa); // Retorna a pessoa encontrada
        }

        /// <summary>
        /// Cadastra uma nova pessoa.
        /// </summary>
        /// <param name="pessoaDto">Os dados da pessoa a ser cadastrada.</param>
        /// <returns>Resposta de sucesso com a pessoa criada ou erro de validação.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Cadastrar nova pessoa", Description = "Este endpoint cria uma nova pessoa.")]
        [ProducesResponseType(typeof(PessoaDto), StatusCodes.Status201Created)]
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

            return CreatedAtAction(nameof(GetPessoa), new { cpf = pessoa.CPF }, pessoa); // Retorno de sucesso com o recurso criado
        }

        /// <summary>
        /// Atualiza os dados de uma pessoa.
        /// </summary>
        /// <param name="cpf">O CPF da pessoa a ser atualizada.</param>
        /// <param name="pessoaDto">Os novos dados da pessoa.</param>
        /// <returns>Resposta de sucesso ou erro de validação.</returns>
        [HttpPut("{cpf}")]
        [SwaggerOperation(Summary = "Atualizar dados de uma pessoa", Description = "Este endpoint permite atualizar os dados de uma pessoa.")]
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

            return NoContent(); // Retorno de sucesso sem conteúdo (indicando que foi atualizado)
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
