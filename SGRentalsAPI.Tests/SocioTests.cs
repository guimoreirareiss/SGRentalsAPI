using Xunit;
using SGRentalsAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace SGRentalsAPI.Tests
{
    public class SocioTests
    {
        [Fact]
        public void Deve_Criar_Socio_Com_Campos_Obrigatorios()
        {

            var socio = new Socio
            {
                Nome = "Gabriel Santos",
                Cpf = "12345678909",
                EmpresaId = 1
            };

            // Act
            var validationContext = new ValidationContext(socio, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(socio, validationContext, validationResults, validateAllProperties: true);

            // Assert
            Assert.True(isValid, $"Validação do sócio falhou: {string.Join(", ", validationResults.Select(r => r.ErrorMessage))}");
            Assert.Equal("Gabriel Santos", socio.Nome);
            Assert.Equal("12345678909", socio.Cpf);
        }

        [Fact]
        public void Nao_Deve_Criar_Socio_Com_Campos_Invalidos()
        {

            var socio = new Socio
            {
                Nome = "",
                Cpf = "11111111111",
                EmpresaId = 1
            };

            var validationContext = new ValidationContext(socio, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(socio, validationContext, validationResults, validateAllProperties: true);

            Assert.False(isValid, "Validação do sócio deveria ter falhado.");
            Assert.Contains("Nome é obrigatório", validationResults.Select(r => r.ErrorMessage));
            Assert.Contains("CPF inválido.", validationResults.Select(r => r.ErrorMessage));
        }
    }
}