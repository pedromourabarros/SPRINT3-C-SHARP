using Microsoft.AspNetCore.Mvc;
using ApostasCompulsivas.DTOs;
using ApostasCompulsivas.Services;
using System.ComponentModel.DataAnnotations;

namespace ApostasCompulsivas.Controllers
{
    /// <summary>
    /// Controller para operações de usuários
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        /// <summary>
        /// Lista todos os usuários
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDTO>>> GetUsuarios()
        {
            try
            {
                var usuarios = await _usuarioService.ListarUsuariosAsync();
                var usuariosDTO = usuarios.Select(u => u.ToDTO()).ToList();
                return Ok(usuariosDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém um usuário por ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDTO>> GetUsuario(int id)
        {
            try
            {
                var usuario = await _usuarioService.ObterUsuarioPorIdAsync(id);
                if (usuario == null)
                    return NotFound($"Usuário com ID {id} não encontrado");

                return Ok(usuario.ToDTO());
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// Cria um novo usuário
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> CriarUsuario([FromBody] CriarUsuarioDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var usuario = await _usuarioService.CriarUsuarioAsync(
                    dto.Nome, 
                    dto.Email, 
                    dto.SaldoInicial, 
                    dto.Telefone
                );

                return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario.ToDTO());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// Atualiza um usuário existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> AtualizarUsuario(int id, [FromBody] AtualizarUsuarioDTO dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var usuarioExistente = await _usuarioService.ObterUsuarioPorIdAsync(id);
                if (usuarioExistente == null)
                    return NotFound($"Usuário com ID {id} não encontrado");

                dto.Id = id;
                var usuarioAtualizado = dto.ToEntity(usuarioExistente);
                await _usuarioService.AtualizarUsuarioAsync(usuarioAtualizado);

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// Desativa um usuário
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DesativarUsuario(int id)
        {
            try
            {
                var sucesso = await _usuarioService.DesativarUsuarioAsync(id);
                if (!sucesso)
                    return NotFound($"Usuário com ID {id} não encontrado");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// Exclui um usuário definitivamente (LGPD)
        /// </summary>
        [HttpDelete("{id}/definitivo")]
        public async Task<IActionResult> ExcluirUsuarioDefinitivamente(int id)
        {
            try
            {
                var sucesso = await _usuarioService.ExcluirUsuarioDefinitivamenteAsync(id);
                if (!sucesso)
                    return NotFound($"Usuário com ID {id} não encontrado");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// Realiza um depósito para um usuário
        /// </summary>
        [HttpPost("{id}/depositar")]
        public async Task<IActionResult> Depositar(int id, [FromBody] decimal valor)
        {
            try
            {
                if (valor <= 0)
                    return BadRequest("Valor deve ser maior que zero");

                var sucesso = await _usuarioService.DepositarAsync(id, valor);
                if (!sucesso)
                    return NotFound($"Usuário com ID {id} não encontrado");

                return Ok(new { message = "Depósito realizado com sucesso", valor });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        /// <summary>
        /// Realiza um saque para um usuário
        /// </summary>
        [HttpPost("{id}/sacar")]
        public async Task<IActionResult> Sacar(int id, [FromBody] decimal valor)
        {
            try
            {
                if (valor <= 0)
                    return BadRequest("Valor deve ser maior que zero");

                var sucesso = await _usuarioService.SacarAsync(id, valor);
                if (!sucesso)
                    return NotFound($"Usuário com ID {id} não encontrado");

                return Ok(new { message = "Saque realizado com sucesso", valor });
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
    }
}
