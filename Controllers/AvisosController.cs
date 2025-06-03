using Microsoft.AspNetCore.Mvc;
using SolutionApi.Models;
using SolutionApi.Data;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace SolutionApi.Controllers
{
    /// <summary>
    /// Controlador para gerenciar os avisos de eventos climáticos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Tags("Avisos")]
    public class AvisoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor do controller de Aviso.
        /// </summary>
        /// <param name="context">Contexto de acesso ao banco de dados.</param>
        public AvisoController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna todos os avisos registrados.
        /// </summary>
        /// <returns>Lista de avisos</returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Retorna todos os avisos", Description = "Retorna uma lista com todos os avisos registrados.")]
        [ProducesResponseType(typeof(IEnumerable<Aviso>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<Aviso>>> GetAvisos()
        {
            var avisos = await _context.Avisos.ToListAsync();
            if (avisos == null || !avisos.Any())
            {
                return NoContent();
            }
            return Ok(avisos);
        }

        /// <summary>
        /// Retorna um aviso específico pelo tipo.
        /// </summary>
        /// <param name="tipoAviso">Tipo do aviso</param>
        /// <returns>Detalhes do aviso</returns>
        [HttpGet("{tipoAviso}")]
        [SwaggerOperation(Summary = "Retorna um aviso pelo tipo", Description = "Retorna um aviso baseado no tipo do evento climático.")]
        [ProducesResponseType(typeof(Aviso), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<ActionResult<Aviso>> GetAviso(string tipoAviso)
        {
            var aviso = await _context.Avisos.FirstOrDefaultAsync(a => a.TipoAviso == tipoAviso);
            if (aviso == null)
            {
                return NotFound(new { message = "Aviso não encontrado." });
            }
            return Ok(aviso);
        }

        /// <summary>
        /// Cria um novo aviso.
        /// </summary>
        /// <param name="aviso">Dados do aviso a ser criado</param>
        /// <returns>Resultado da criação</returns>
        [HttpPost]
        [SwaggerOperation(Summary = "Cria um novo aviso", Description = "Este endpoint cria um novo aviso com as informações fornecidas.")]
        [ProducesResponseType(typeof(Aviso), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<IActionResult> PostAviso([FromBody] Aviso aviso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos.", errors = ModelState });
            }

            _context.Avisos.Add(aviso);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetAviso), new { tipoAviso = aviso.TipoAviso }, aviso);
        }

        /// <summary>
        /// Atualiza um aviso existente.
        /// </summary>
        /// <param name="tipoAviso">Tipo do aviso</param>
        /// <param name="aviso">Dados atualizados do aviso</param>
        /// <returns>Resultado da atualização</returns>
        [HttpPut("{tipoAviso}")]
        [SwaggerOperation(Summary = "Atualiza um aviso existente", Description = "Atualiza as informações de um aviso com base no tipo do evento climático.")]
        [ProducesResponseType(typeof(Aviso), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> PutAviso(string tipoAviso, [FromBody] Aviso aviso)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Dados inválidos.", errors = ModelState });
            }

            var existingAviso = await _context.Avisos.FirstOrDefaultAsync(a => a.TipoAviso == tipoAviso);
            if (existingAviso == null)
            {
                return NotFound(new { message = "Aviso não encontrado." });
            }

            existingAviso.Ocorrencia = aviso.Ocorrencia;
            existingAviso.Gravidade = aviso.Gravidade;
            existingAviso.Bairro = aviso.Bairro;

            _context.Avisos.Update(existingAviso);
            await _context.SaveChangesAsync();
            return Ok(existingAviso);
        }

        /// <summary>
        /// Deleta um aviso existente.
        /// </summary>
        /// <param name="tipoAviso">Tipo do aviso</param>
        /// <returns>Resultado da exclusão</returns>
        [HttpDelete("{tipoAviso}")]
        [SwaggerOperation(Summary = "Deleta um aviso", Description = "Deleta um aviso existente com base no tipo do evento climático.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> DeleteAviso(string tipoAviso)
        {
            var aviso = await _context.Avisos.FirstOrDefaultAsync(a => a.TipoAviso == tipoAviso);
            if (aviso == null)
            {
                return NotFound(new { message = "Aviso não encontrado." });
            }

            _context.Avisos.Remove(aviso);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
