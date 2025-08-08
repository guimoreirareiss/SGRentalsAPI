using SGRentalsAPI.Data;
using SGRentalsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace SGRentalsAPI.Services
{
    public class UsuarioService
    {
        private readonly ApplicationDbContext _context;

        public UsuarioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> AddUsuarioAsync(Usuario usuario)
        {
            ValidateModel(usuario);

            // Verifica se já existe um usuário com o mesmo CPF na mesma empresa
            var existingUser = await _context.Usuarios
                                             .FirstOrDefaultAsync(u => u.Cpf == usuario.Cpf && u.EmpresaId == usuario.EmpresaId);
            if (existingUser != null)
            {
                throw new ValidationException("Já existe um usuário com este CPF cadastrado nesta empresa.");
            }

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario?> GetUsuarioByIdAsync(int id)
        {
            return await _context.Usuarios
                                 .Include(u => u.Empresa)
                                 .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<Usuario>> GetAllUsuariosAsync()
        {
            return await _context.Usuarios
                                 .Include(u => u.Empresa)
                                 .ToListAsync();
        }

        public async Task<Usuario?> UpdateUsuarioAsync(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                throw new ArgumentException("ID do usuário não corresponde.");
            }

            ValidateModel(usuario);

            // Verifica se o CPF já existe para outro usuário na mesma empresa
            var existingUser = await _context.Usuarios
                                             .Where(u => u.Cpf == usuario.Cpf && u.EmpresaId == usuario.EmpresaId && u.Id != usuario.Id)
                                             .FirstOrDefaultAsync();
            if (existingUser != null)
            {
                throw new ValidationException("Já existe outro usuário com este CPF cadastrado nesta empresa.");
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Usuarios.AnyAsync(u => u.Id == id))
                {
                    return null;
                }
                throw;
            }
            return usuario;
        }

        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return false;
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Usuario?> InactivateUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return null;
            }

            usuario.IsActive = false;
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosByPerfilAsync(PerfilUsuario perfil)
        {
            return await _context.Usuarios
                                 .Include(u => u.Empresa)
                                 .Where(u => u.Perfil == perfil)
                                 .ToListAsync();
        }

        private void ValidateModel(object model)
        {
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(model, validationContext, validationResults, validateAllProperties: true))
            {
                throw new ValidationException(string.Join("; ", validationResults.Select(r => r.ErrorMessage)));
            }
        }
    }
}