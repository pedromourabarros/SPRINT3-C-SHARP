using Microsoft.AspNetCore.Mvc;
using ApostasCompulsivas.DTOs;
using ApostasCompulsivas.Services;

namespace ApostasCompulsivas.Controllers
{
    /// <summary>
    /// Controller para operações de apostas
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ApostasController : ControllerBase
    {
        private readonly IApostaService _apostaService;

        public ApostasController(IApostaService apostaService)
        {
            _apostaService = apostaService;
        }

        /// <summary>
        /// Lista todas as apostas
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApostaDTO>>> GetApostas()
        {
            try
            {
                var apostas = await _apostaService.ListarApostasAsync();
                var apostasDTO = apostas.Select(a => a.ToDTO()).ToList();
                return Ok(apostasDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém uma aposta por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApostaDTO>> GetAposta(int id)
        {
            try
            {
                var aposta = await _apostaService.ObterApostaPorIdAsync(id);
                if (aposta == null)
                    return NotFound($"Aposta com ID {id} não encontrada");

                return Ok(aposta.ToDTO());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// Cria uma nova aposta
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApostaDTO>> CriarAposta([FromBody] CriarApostaDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var aposta = await _apostaService.RealizarApostaAsync(
                    dto.UsuarioId,
                    dto.TipoAposta,
                    dto.Valor,
                    dto.Multiplicador
                );

                return CreatedAtAction(nameof(GetAposta), new { id = aposta.Id }, aposta.ToDTO());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// Finaliza uma aposta
        /// </summary>
        [HttpPost("{id}/finalizar")]
        public async Task<ActionResult<ApostaDTO>> FinalizarAposta(int id, [FromBody] FinalizarApostaDTO dto)
        {
            try
            {
                if (dto.ApostaId != id)
                    return BadRequest("ID da aposta não confere");

                var aposta = await _apostaService.FinalizarApostaAsync(id, dto.Ganhou);
                if (aposta == null)
                    return NotFound($"Aposta com ID {id} não encontrada");

                return Ok(aposta.ToDTO());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// Lista apostas por usuário
        /// </summary>
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<ApostaDTO>>> GetApostasPorUsuario(int usuarioId)
        {
            try
            {
                var apostas = await _apostaService.ObterApostasPorUsuarioAsync(usuarioId);
                var apostasDTO = apostas.Select(a => a.ToDTO()).ToList();
                return Ok(apostasDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// Lista apostas por status
        /// </summary>
        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<ApostaDTO>>> GetApostasPorStatus(string status)
        {
            try
            {
                var apostas = await _apostaService.ObterApostasPorStatusAsync(status);
                var apostasDTO = apostas.Select(a => a.ToDTO()).ToList();
                return Ok(apostasDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}
