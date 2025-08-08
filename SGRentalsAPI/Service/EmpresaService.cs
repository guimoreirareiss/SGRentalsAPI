using SGRentalsAPI.Data;
using SGRentalsAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace SGRentalsAPI.Services
{
    public class EmpresaService
    {
        private readonly ApplicationDbContext _context;

        public EmpresaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Empresa?> AddEmpresaAsync(Empresa empresa)
        {
            // Validação de modelo usando Data Annotations
            ValidateModel(empresa);

            // Verifica se o CNPJ já existe no banco de dados
            if (await _context.Empresas.AnyAsync(e => e.Cnpj == empresa.Cnpj))
            {
                throw new ValidationException("Já existe uma empresa cadastrada com este CNPJ.");
            }

            // O EF Core irá rastrear e adicionar o Endereço automaticamente se ele for uma nova entidade (Id == 0)
            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();
            return empresa;
        }

        public async Task<Empresa?> GetEmpresaByIdAsync(int id)
        {
            return await _context.Empresas
                                 .Include(e => e.Endereco)
                                 .Include(e => e.Socios)
                                 .Include(e => e.Usuarios)
                                 .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<Empresa>> GetAllEmpresasAsync()
        {
            return await _context.Empresas
                                 .Include(e => e.Endereco)
                                 .Include(e => e.Socios)
                                 .Include(e => e.Usuarios)
                                 .ToListAsync();
        }

        public async Task<Empresa?> UpdateEmpresaAsync(int id, Empresa empresa)
        {
            if (id != empresa.Id)
            {
                throw new ArgumentException("ID da empresa não corresponde.");
            }

            // Validação de modelo
            ValidateModel(empresa);

            // Verifica se o CNPJ já existe para outra empresa
            if (await _context.Empresas.AnyAsync(e => e.Cnpj == empresa.Cnpj && e.Id != id))
            {
                throw new ValidationException("Já existe outra empresa cadastrada com este CNPJ.");
            }

            // Busca a empresa existente para garantir o tratamento correto do Endereço
            var existingEmpresa = await _context.Empresas.Include(e => e.Endereco).FirstOrDefaultAsync(e => e.Id == id);
            if (existingEmpresa == null)
            {
                return null; // Empresa não encontrada
            }
            
            // Atualiza as propriedades da entidade
            _context.Entry(existingEmpresa).CurrentValues.SetValues(empresa);

            // Lida com o Endereço
            if (empresa.Endereco != null)
            {
                if (existingEmpresa.Endereco != null)
                {
                    // Atualiza o endereço existente
                    _context.Entry(existingEmpresa.Endereco).CurrentValues.SetValues(empresa.Endereco);
                }
                else
                {
                    // Adiciona um novo endereço se não houver um
                    _context.Enderecos.Add(empresa.Endereco);
                    existingEmpresa.Endereco = empresa.Endereco;
                }
            }
            else if (existingEmpresa.Endereco != null)
            {
                // Remove o endereço antigo se não for mais referenciado
                _context.Enderecos.Remove(existingEmpresa.Endereco);
                existingEmpresa.Endereco = null;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Empresas.AnyAsync(e => e.Id == id))
                {
                    return null; // Empresa não encontrada
                }
                throw;
            }
            return existingEmpresa;
        }

        public async Task<bool> DeleteEmpresaAsync(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return false;
            }

            try
            {
                _context.Empresas.Remove(empresa);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException ex)
            {
                // Tratar a exceção se a exclusão for restrita (Devido a FKs)
                throw new InvalidOperationException("Não é possível excluir a empresa pois ela possui usuários ou sócios associados.", ex);
            }
        }

        public async Task<Empresa?> InactivateEmpresaAsync(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);
            if (empresa == null)
            {
                return null;
            }

            empresa.IsActive = false;
            _context.Entry(empresa).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return empresa;
        }

        public async Task<IEnumerable<Empresa>> GetEmpresasByTypeAsync(string tipoEmpresa)
        {
            return await _context.Empresas
                                 .Include(e => e.Endereco)
                                 .Include(e => e.Socios)
                                 .Include(e => e.Usuarios)
                                 .Where(e => e.TipoEmpresa == tipoEmpresa)
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