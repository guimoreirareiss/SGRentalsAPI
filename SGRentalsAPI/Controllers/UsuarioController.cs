using Microsoft.AspNetCore.Mvc;
using SGRentalsAPI; // Importa suas classes de modelo (namespace SGRentalsAPI)
using SGRentalsAPI.Services; // Importa o namespace dos serviços
using System.ComponentModel.DataAnnotations; // Para ValidationException
using SGRentalsAPI.Models;

namespace SGRentalsAPI.Controllers // Namespace para a camada de controladores
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // Requisito: Consultar (por ID)
        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _usuarioService.GetUsuarioByIdAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // Requisito: Consultar (todos)
        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return Ok(await _usuarioService.GetAllUsuariosAsync());
        }

        // Requisito: Adicionar
        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            try
            {
                var newUsuario = await _usuarioService.AddUsuarioAsync(usuario);
                return CreatedAtAction(nameof(GetUsuario), new { id = newUsuario?.Id }, newUsuario);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        // Requisito: Editar
        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            try
            {
                var updatedUsuario = await _usuarioService.UpdateUsuarioAsync(id, usuario);
                if (updatedUsuario == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
            }
        }

        // Requisito: Excluir
        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var result = await _usuarioService.DeleteUsuarioAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Requisito: Inativar
        // PUT: api/Usuarios/inativar/5
        [HttpPut("inativar/{id}")]
        public async Task<IActionResult> InativarUsuario(int id)
        {
            try
            {
                var usuario = await _usuarioService.InactivateUsuarioAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao inativar usuário: {ex.Message}");
            }
        }

        // Requisito: Consultar usuários por perfil
        // GET: api/Usuarios/por-perfil?perfil=Administrador
        [HttpGet("por-perfil")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuariosPorPerfil([FromQuery] PerfilUsuario perfil)
        {
            var usuarios = await _usuarioService.GetUsuariosByPerfilAsync(perfil);
            if (!usuarios.Any())
            {
                return NotFound($"Nenhum usuário encontrado para o perfil: {perfil}");
            }
            return Ok(usuarios);
        }
    }
}