// Este controlador foi otimizado para lidar com a validação de CPF e as operações CRUD para sócios.
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGRentalsAPI.Data;
using SGRentalsAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SGRentalsAPI.Controllers
{
    // Define a classe como um controlador de API
    [ApiController]
    // Define a rota base para o controlador
    [Route("api/[controller]")]
    public class SocioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Construtor que injeta a dependência do ApplicationDbContext
        public SocioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/Socio
        // Endpoint para criar um novo sócio
        [HttpPost]
        public async Task<ActionResult<Socio>> PostSocio(Socio socio)
        {
            // Valida o objeto 'socio' usando os atributos de validação do modelo.
            // Isso inclui a sua classe CpfValidationAttribute.
            var validationContext = new ValidationContext(socio, null, null);
            var validationResults = new System.Collections.Generic.List<ValidationResult>();

            if (!Validator.TryValidateObject(socio, validationContext, validationResults, true))
            {
                // Se a validação falhar, retorna um erro 400 Bad Request com os erros encontrados.
                return BadRequest(new { errors = validationResults.Select(r => r.ErrorMessage) });
            }

            // Adiciona o novo sócio ao contexto do banco de dados
            _context.Socios.Add(socio);
            // Salva as mudanças de forma assíncrona
            await _context.SaveChangesAsync();

            // Retorna a resposta HTTP 201 Created com o sócio criado
            return CreatedAtAction(nameof(GetSocio), new { id = socio.Id }, socio);
        }

        // GET: api/Socio/{id}
        // Endpoint para buscar um sócio específico por ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Socio>> GetSocio(int id)
        {
            // Busca o sócio no banco de dados pelo ID
            var socio = await _context.Socios.FindAsync(id);

            if (socio == null)
            {
                // Se o sócio não for encontrado, retorna um erro 404 Not Found
                return NotFound();
            }

            // Se encontrado, retorna o sócio com o status 200 OK
            return socio;
        }

        // DELETE: api/Socio/{id}
        // Endpoint para excluir um sócio por ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSocio(int id)
        {
            // Busca o sócio no banco de dados
            var socio = await _context.Socios.FindAsync(id);
            if (socio == null)
            {
                // Se não for encontrado, retorna 404 Not Found
                return NotFound();
            }

            // Remove o sócio do contexto
            _context.Socios.Remove(socio);
            // Salva as alterações
            await _context.SaveChangesAsync();

            // Retorna 204 No Content para indicar que a exclusão foi bem-sucedida
            return NoContent();
        }
    }
}