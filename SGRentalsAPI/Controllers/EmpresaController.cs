using Microsoft.AspNetCore.Mvc;
using SGRentalsAPI;
using SGRentalsAPI.Services;
using System.ComponentModel.DataAnnotations;
using SGRentalsAPI.Models;

namespace SGRentalsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpresaController : ControllerBase
    {
        private readonly EmpresaService _empresaService;

        public EmpresaController(EmpresaService empresaService)
        {
            _empresaService = empresaService;
        }

        // Requisito: Consultar (por ID)
        // GET: api/Empresa/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Empresa>> GetEmpresa(int id)
        {
            var empresa = await _empresaService.GetEmpresaByIdAsync(id);

            if (empresa == null)
            {
                return NotFound();
            }

            return empresa;
        }

        // Requisito: Consultar (todas)
        // GET: api/Empresa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empresa>>> GetEmpresa()
        {
            return Ok(await _empresaService.GetAllEmpresasAsync());
        }

        // Requisito: Adicionar
        // POST: api/Empresa
        [HttpPost]
        public async Task<ActionResult<Empresa>> PostEmpresa(Empresa empresa)
        {
            try
            {
                var newEmpresa = await _empresaService.AddEmpresaAsync(empresa);
                return CreatedAtAction(nameof(GetEmpresa), new { id = newEmpresa?.Id }, newEmpresa);
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
        // PUT: api/Empresa/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpresa(int id, Empresa empresa)
        {
            try
            {
                var updatedEmpresa = await _empresaService.UpdateEmpresaAsync(id, empresa);
                if (updatedEmpresa == null)
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
        // DELETE: api/Empresa/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpresa(int id)
        {
            var result = await _empresaService.DeleteEmpresaAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        // Requisito: Inativar
        // PUT: api/Empresa/inativar/5
        [HttpPut("inativar/{id}")]
        public async Task<IActionResult> InativarEmpresa(int id)
        {
            try
            {
                var empresa = await _empresaService.InactivateEmpresaAsync(id);
                if (empresa == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao inativar empresa: {ex.Message}");
            }
        }

        // Requisito: Consultar empresas por tipo
        [HttpGet("por-tipo")]
        public async Task<ActionResult<IEnumerable<Empresa>>> GetEmpresasPorTipo([FromQuery] string tipo)
        {
            if (string.IsNullOrWhiteSpace(tipo))
            {
                return BadRequest("O tipo de empresa é obrigatório para a consulta.");
            }
            var empresas = await _empresaService.GetEmpresasByTypeAsync(tipo);
            if (!empresas.Any())
            {
                return NotFound($"Nenhuma empresa encontrada para o tipo: {tipo}");
            }
            return Ok(empresas);
        }
    }
}