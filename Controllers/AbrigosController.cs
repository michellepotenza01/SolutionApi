using Microsoft.AspNetCore.Mvc;
using SolutionApi.DTOs;
using SolutionApi.Models;
using SolutionApi.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace SolutionApi.Controllers
{
    /// <summary>
    /// Controlador para gerenciar as operações dos abrigos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Abrigos")]
    public class AbrigoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor do controller de Abrigo.
        /// </summary>
        /// <param name="context">Contexto de acesso ao banco de dados.</param>
        public AbrigoController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os abrigos registrados.
        /// </summary>
        /// <returns>Lista de abrigos</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Retorna todos os abrigos", Description = "Este endpoint retorna uma lista com todos os abrigos registrados.")]
        [ProducesResponseType(typeof(IEnumerable<AbrigoDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Abrigo>>> GetAbrigos()
        {
            var abrigos = await _context.Abrigos
                .Include(a => a.Voluntarios) // Inclui os voluntários associados ao abrigo
                .ToListAsync();

            if (!abrigos.Any())
            {
                return NoContent(); // Retorna 204 No Content caso não haja abrigos
            }

            // Criação de uma resposta limpa e estruturada
            var response = abrigos.Select(abrigo => new AbrigoDto
            {
                NomeAbrigo = abrigo.NomeAbrigo,
                Bairro = abrigo.Bairro,
                Tamanho = abrigo.Tamanho,
                Voluntarios = [.. abrigo.Voluntarios.Select(voluntario => new VoluntarioDto
                {
                    RG = voluntario.RG,
                    CPF = voluntario.CPF,
                    Funcao = voluntario.Funcao
                })]
            }).ToList();

            return Ok(new { message = "Lista de abrigos carregada com sucesso.", data = response });
        }


        /// <summary>
        /// Retorna um abrigo específico pelo nome.
        /// </summary>
        /// <param name="nomeAbrigo">Nome do abrigo</param>
        /// <returns>Detalhes do abrigo</returns>
        [HttpGet("{nomeAbrigo}")]
        [SwaggerOperation(Summary = "Retorna um abrigo pelo nome.", Description = "Este endpoint retorna um abrigo baseado no nome.")]
        [ProducesResponseType(typeof(Abrigo), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<Abrigo>> GetAbrigo(string nomeAbrigo)
        {
            var abrigo = await _context.Abrigos
                .Include(a => a.Voluntarios) // Inclui voluntários caso haja
                .FirstOrDefaultAsync(a => a.NomeAbrigo == nomeAbrigo);

            if (abrigo == null)
            {
                return NotFound(new { message = "Abrigo não encontrado." });
            }

            var response = new
            {
                message = $"Abrigo: {abrigo.NomeAbrigo} carregado com sucesso.",
                data = new
                {
                    abrigo.NomeAbrigo,
                    abrigo.Bairro,
                    abrigo.Tamanho,
                    voluntarios = abrigo.Voluntarios.Select(v => new
                    {
                        v.RG,
                        v.CPF,
                        v.Funcao
                    }).ToList()
                }
            };

            return Ok(response);
        }

        /// <summary>
        /// Cria um novo abrigo.
        /// </summary>
        /// <param name="abrigoDto">Dados do abrigo a ser criado</param>
        /// <returns>Resultado da criação</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Cria um novo abrigo.", Description = "Este endpoint cria um novo abrigo com as informações fornecidas.")]
        [ProducesResponseType(typeof(AbrigoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
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
                Tamanho = abrigoDto.Tamanho,
            };

            _context.Abrigos.Add(abrigo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAbrigo), new { nomeAbrigo = abrigo.NomeAbrigo }, new { message = "Abrigo criado com sucesso.", data = abrigo });
        }


        /// <summary>
        /// Atualiza um abrigo existente.
        /// </summary>
        /// <param name="nomeAbrigo">Nome do abrigo</param>
        /// <param name="abrigoDto">Dados atualizados do abrigo</param>
        /// <returns>Resultado da atualização</returns>
        [HttpPut("{nomeAbrigo}")]
        [SwaggerOperation(Summary = "Atualiza um abrigo existente.", Description = "Este endpoint atualiza as informações de um abrigo com base no nome.")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> UpdateAbrigo(string nomeAbrigo, [FromBody] AbrigoDto abrigoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos.", errors = ModelState });
            }

            var abrigo = await _context.Abrigos.FirstOrDefaultAsync(a => a.NomeAbrigo == nomeAbrigo);

            if (abrigo == null)
            {
                return NotFound(new { message = "Abrigo não encontrado." });
            }

            abrigo.Bairro = abrigoDto.Bairro;
            abrigo.Tamanho = abrigoDto.Tamanho;

            _context.Abrigos.Update(abrigo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Abrigo atualizado com sucesso.", data = abrigo });
        }

        /// <summary>
        /// Deleta um abrigo existente.
        /// </summary>
        /// <param name="nomeAbrigo">Nome do abrigo</param>
        /// <returns>Resultado da exclusão</returns>
        [HttpDelete("{nomeAbrigo}")]
        [SwaggerOperation(Summary = "Deleta um abrigo.", Description = "Este endpoint deleta um abrigo existente com base no nome.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteAbrigo(string nomeAbrigo)
        {
            var abrigo = await _context.Abrigos.FirstOrDefaultAsync(a => a.NomeAbrigo == nomeAbrigo);

            if (abrigo == null)
            {
                return NotFound(new { message = "Abrigo não encontrado." });
            }

            _context.Abrigos.Remove(abrigo);
            await _context.SaveChangesAsync();

            return NoContent(); // Retorna sucesso sem conteúdo
        }

        /// <summary>
        /// Retorna todos os voluntários associados a um abrigo.
        /// </summary>
        /// <param name="nomeAbrigo">Nome do abrigo</param>
        /// <returns>Lista de voluntários associados ao abrigo</returns>
        [HttpGet("{nomeAbrigo}/voluntarios")]
        [SwaggerOperation(Summary = "Listar voluntários associados a um abrigo", Description = "Este endpoint retorna todos os voluntários associados a um abrigo pelo nome.")]
        [ProducesResponseType(typeof(IEnumerable<VoluntarioDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> GetVoluntariosByAbrigo(string nomeAbrigo)
        {
            var abrigo = await _context.Abrigos.FirstOrDefaultAsync(a => a.NomeAbrigo == nomeAbrigo);

            if (abrigo == null)
            {
                return NotFound(new { message = "Abrigo não encontrado." });
            }

            var voluntarios = await _context.Voluntarios
                .Where(v => v.NomeAbrigo == nomeAbrigo)
                .ToListAsync();

            return Ok(new { message = "Voluntários associados ao abrigo carregados com sucesso.", data = voluntarios });
        }
    }
}
