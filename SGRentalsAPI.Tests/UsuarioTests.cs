using Xunit;
    using SGRentalsAPI.Models;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using System.Linq;

    namespace SGRentalsAPI.Tests
    {
        public class UsuariosTests
        {
            [Fact]
            public void Deve_Criar_Usuario_Com_Campos_Obrigatorios()
            {
                var usuario = new Usuario
                {
                    Nome = "Willian Moura",
                    Email = "willianmouradossantos@gmail.com",
                    SenhaHash = "40028922",
                    SenhaSalt = "saltAleatório",
                    Cpf = "96374185200",
                    Perfil = PerfilUsuario.Colaborador,
                    EmpresaId = 7
                };

                var validationContext = new ValidationContext(usuario, serviceProvider: null, items: null);
                var validationResults = new List<ValidationResult>();
                var isValid = Validator.TryValidateObject(usuario, validationContext, validationResults, validateAllProperties: true);

                Assert.True(isValid, $"Validação do usuário falhou: {string.Join(", ", validationResults.Select(r => r.ErrorMessage))}");

                // Verifica se as propriedades foram atribuídas corretamente
                Assert.Equal("Willian Moura", usuario.Nome);
                Assert.Equal("willianmouradossantos@gmail.com", usuario.Email);
                Assert.Equal("96374185200", usuario.Cpf);
                Assert.Equal(PerfilUsuario.Colaborador, usuario.Perfil);
            }
        }
    }
    