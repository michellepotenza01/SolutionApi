using Microsoft.AspNetCore.Mvc;
using SolutionApi.DTOs;
using SolutionApi.Models;
using SolutionApi.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace SolutionApi.Controllers
{
    /// <summary>
    /// Controller para gerenciar os voluntários.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Voluntários")]
    //[ApiExplorerSettings(GroupName = "Voluntários")]
    //[Tags("Voluntários")]
    public class VoluntarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor do controller de voluntários.
        /// </summary>
        /// <param name="context">Contexto de acesso ao banco de dados.</param>
        public VoluntarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os voluntários cadastrados.
        /// </summary>
        /// <returns>Uma lista de voluntários ou uma resposta de 'No Content' se não houver voluntários.</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Listar todos os voluntários", Description = "Este endpoint retorna todos os voluntários cadastrados.")]
        [ProducesResponseType(typeof(IEnumerable<VoluntarioDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Voluntario>>> GetVoluntarios()
        {
            var voluntarios = await _context.Voluntarios
                .Include(v => v.Pessoa)
                .Include(v => v.Abrigo)
                .ToListAsync();

            if (!voluntarios.Any())
            {
                return NoContent(); // Retorno caso não haja voluntários cadastrados
            }

            return Ok(voluntarios); // Retorna a lista de voluntários
        }

        /// <summary>
        /// Retorna um voluntário pelo CPF.
        /// </summary>
        /// <param name="cpf">O CPF do voluntário.</param>
        /// <returns>O voluntário encontrado ou uma resposta de 'Not Found' se não encontrar.</returns>
        [HttpGet("{cpf}")]
        [SwaggerOperation(Summary = "Buscar voluntário por CPF", Description = "Este endpoint busca um voluntário através do CPF.")]
        [ProducesResponseType(typeof(VoluntarioDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<Voluntario>> GetVoluntario(string cpf)
        {
            var voluntario = await _context.Voluntarios
                .Include(v => v.Pessoa)
                .Include(v => v.Abrigo)
                .FirstOrDefaultAsync(v => v.CPF == cpf);

            if (voluntario == null)
            {
                return NotFound(new { message = "Voluntário não encontrado com o CPF fornecido." });
            }

            return Ok(voluntario); // Retorna o voluntário encontrado
        }

        /// <summary>
        /// Cadastra um novo voluntário.
        /// </summary>
        /// <param name="voluntarioDto">Os dados do voluntário a ser cadastrado.</param>
        /// <returns>Resposta de sucesso com o voluntário criado ou erro de validação.</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Cadastrar novo voluntário", Description = "Este endpoint cria um novo voluntário. Deve associar o CPF de uma pessoa existente.")]
        [ProducesResponseType(typeof(VoluntarioDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> PostVoluntario([FromBody] VoluntarioDto voluntarioDto)
        {
            // Verificando se o modelo está correto
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos.", errors = ModelState });
            }

            // Encontrando a pessoa pelo CPF
            var pessoa = await _context.Pessoas.FindAsync(voluntarioDto.CPF);
            if (pessoa == null)
            {
                return BadRequest(new { message = "Pessoa não encontrada para associar ao voluntário." });
            }

            // Criando o voluntário
            var voluntario = new Voluntario
            {
                RG = voluntarioDto.RG,
                CPF = voluntarioDto.CPF,
                Funcao = voluntarioDto.Funcao,
                NomeAbrigo = voluntarioDto.NomeAbrigo,
                Pessoa = pessoa // Associando a pessoa ao voluntário
            };

            // Adicionando e salvando no banco
            _context.Voluntarios.Add(voluntario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVoluntario), new { cpf = voluntario.CPF }, voluntario); // Retorno com o voluntário criado
        }


        /// <summary>
        /// Atualiza os dados de um voluntário.
        /// </summary>
        /// <param name="cpf">O CPF do voluntário a ser atualizado.</param>
        /// <param name="voluntarioDto">Os novos dados do voluntário.</param>
        /// <returns>Resposta de sucesso ou erro de validação.</returns>
        [HttpPut("{cpf}")]
        [SwaggerOperation(Summary = "Atualizar dados de um voluntário", Description = "Este endpoint permite atualizar os dados de um voluntário associado ao CPF.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> PutVoluntario(string cpf, [FromBody] VoluntarioDto voluntarioDto)
        {
            var voluntario = await _context.Voluntarios.FirstOrDefaultAsync(v => v.CPF == cpf);
            if (voluntario == null)
            {
                return NotFound(new { message = "Voluntário não encontrado para atualização." });
            }

            voluntario.RG = voluntarioDto.RG;
            voluntario.Funcao = voluntarioDto.Funcao;
            voluntario.NomeAbrigo = voluntarioDto.NomeAbrigo;

            _context.Entry(voluntario).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent(); // Confirmação de sucesso sem conteúdo
        }

        /// <summary>
        /// Deleta um voluntário.
        /// </summary>
        /// <param name="cpf">O CPF do voluntário a ser excluído.</param>
        /// <returns>Resposta de sucesso ou erro se não encontrar o voluntário.</returns>
        [HttpDelete("{cpf}")]
        [SwaggerOperation(Summary = "Deletar um voluntário", Description = "Este endpoint exclui um voluntário do sistema com base no CPF.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteVoluntario(string cpf)
        {
            var voluntario = await _context.Voluntarios.FirstOrDefaultAsync(v => v.CPF == cpf);
            if (voluntario == null)
            {
                return NotFound(new { message = "Voluntário não encontrado para exclusão." });
            }

            _context.Voluntarios.Remove(voluntario);
            await _context.SaveChangesAsync();

            return NoContent(); // Confirmação de exclusão
        }
    }
}
