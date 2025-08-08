using Xunit;
using SGRentalsAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace SGRentalsAPI.Tests
{
    // Classe de teste para o atributo de validação de CNPJ
    public class CnpjValidationAttributeTests
    {
        // Classe auxiliar para aplicar o atributo de validação de CNPJ
        private class TesteObjetoComCnpj
        {
            [CnpjValidation(ErrorMessage = "CNPJ inválido.")]
            public string Cnpj { get; set; } = string.Empty;
        }

        [Fact]
        public void CnpjValido_Deve_Passar_Na_Validacao()
        {

            // Usando um CNPJ que é matematicamente válido
            var obj = new TesteObjetoComCnpj { Cnpj = "00000000000191" }; // CNPJ válido

            var validationContext = new ValidationContext(obj, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, validateAllProperties: true);

            Assert.True(isValid, $"O CNPJ válido deveria ter passado na validação: {string.Join(", ", validationResults.Select(r => r.ErrorMessage))}");
        }

        [Fact]
        public void CnpjInvalido_DigitosRepetidos_Deve_Falhar_Na_Validacao()
        {

            // CNPJ inválido (todos os dígitos iguais)
            var obj = new TesteObjetoComCnpj { Cnpj = "11111111111111" };

            var validationContext = new ValidationContext(obj, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, validateAllProperties: true);

            Assert.False(isValid, "O CNPJ com dígitos repetidos deveria ter falhado na validação.");
            Assert.Contains("CNPJ inválido", validationResults.Select(r => r.ErrorMessage));
        }

        [Fact]
        public void CnpjInvalido_DigitosVerificadoresErrados_Deve_Falhar_Na_Validacao()
        {

            // CNPJ inválido (dígitos verificadores errados)
            var obj = new TesteObjetoComCnpj { Cnpj = "12345678000100" }; // CNPJ com DV errado

            var validationContext = new ValidationContext(obj, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, validateAllProperties: true);

            Assert.False(isValid, "O CNPJ com dígitos verificadores errados deveria ter falhado na validação.");
            Assert.Contains("CNPJ inválido.", validationResults.Select(r => r.ErrorMessage));
        }

        [Fact]
        public void CnpjTamanhoInvalido_MenosDigitos_Deve_Falhar_Na_Validacao()
        {

            // CNPJ com menos de 14 dígitos
            var obj = new TesteObjetoComCnpj { Cnpj = "1234567890123" };

            var validationContext = new ValidationContext(obj, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, validateAllProperties: true);

            Assert.False(isValid, "O CNPJ com tamanho inválido deveria ter falhado na validação.");
            Assert.Contains("CNPJ deve ter 14 dígitos.", validationResults.Select(r => r.ErrorMessage));
        }

        [Fact]
        public void CnpjTamanhoInvalido_MaisDigitos_Deve_Falhar_Na_Validacao()
        {

            // CNPJ com mais de 14 dígitos
            var obj = new TesteObjetoComCnpj { Cnpj = "123456789012345" };

            // Act
            var validationContext = new ValidationContext(obj, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, validateAllProperties: true);

            Assert.False(isValid, "O CNPJ com tamanho inválido deveria ter falhado na validação.");
            Assert.Contains("CNPJ deve ter 14 dígitos.", validationResults.Select(r => r.ErrorMessage));
        }

        [Fact]
        public void CnpjVazio_Deve_Falhar_Na_Validacao()
        {

            var obj = new TesteObjetoComCnpj { Cnpj = "" };

            var validationContext = new ValidationContext(obj, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, validateAllProperties: true);

            Assert.False(isValid, "O CNPJ vazio deveria ter falhado na validação.");
            Assert.Contains("CNPJ não pode ser vazio.", validationResults.Select(r => r.ErrorMessage));
        }

        [Fact]
        public void CnpjNulo_Deve_Passar_Na_Validacao_Se_Nao_For_Required()
        {
    
            var obj = new TesteObjetoComCnpj { Cnpj = null! };

            var validationContext = new ValidationContext(obj, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, validateAllProperties: true);

            Assert.True(isValid, "O CNPJ nulo deveria ter passado na validação do CnpjValidationAttribute (se não fosse Required).");
        }
    }
}