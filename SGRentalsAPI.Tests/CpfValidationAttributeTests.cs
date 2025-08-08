using Xunit;
using SGRentalsAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace SGRentalsAPI.Tests
{

    public class CpfValidationAttributeTests
    {

        private class TesteObjetoComCpf
        {
            [CpfValidation(ErrorMessage = "CPF inválido.")]
            public string Cpf { get; set; } = string.Empty;
        }

        [Fact]
        public void CpfValido_Deve_Passar_Na_Validacao()
        {


            var obj = new TesteObjetoComCpf { Cpf = "12345678909" };

            var validationContext = new ValidationContext(obj, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, validateAllProperties: true);

            Assert.True(isValid, $"O CPF válido deveria ter passado na validação: {string.Join(", ", validationResults.Select(r => r.ErrorMessage))}");
        }
        [Fact]
        public void CpfInvalido_DigitosRepetidos_Deve_Falhar_Na_Validacao()
        {

            var obj = new TesteObjetoComCpf { Cpf = "11111111111" };

            var validationContext = new ValidationContext(obj, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, validateAllProperties: true);

            Assert.False(isValid, "O CPF com dígitos repetidos deveria ter falhado na validação.");
            Assert.Contains("CPF inválido.", validationResults.Select(r => r.ErrorMessage));
        }
        [Fact]
        public void CpfInvalido_DigitosVerificadoresErrados_Deve_Falhar_Na_Validacao()
        {
          
            var obj = new TesteObjetoComCpf { Cpf = "12345678900" };

            var validationContext = new ValidationContext(obj, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, validateAllProperties: true);

         
            Assert.False(isValid, "O CPF com dígitos verificadores errados deveria ter falhado na validação.");
            Assert.Contains("CPF inválido", validationResults.Select(r => r.ErrorMessage));
        }

        [Fact]
        public void CpfVazio_Deve_Falhar_Na_Validacao()
        {
  
            var obj = new TesteObjetoComCpf { Cpf = "" };

            var validationContext = new ValidationContext(obj, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, validateAllProperties: true);

            Assert.False(isValid, "O CPF vazio deveria ter falhado na validação.");
            Assert.Contains("CPF não pode ser vazio.", validationResults.Select(r => r.ErrorMessage));
        }

        [Fact]
        public void CpfNulo_Deve_Passar_Na_Validacao_Se_Nao_For_Required()
        {
        
            var obj = new TesteObjetoComCpf { Cpf = null! };

            var validationContext = new ValidationContext(obj, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(obj, validationContext, validationResults, validateAllProperties: true);

            Assert.True(isValid, "O CPF nulo deveria ter passado na validação do CpfValidationAttribute (se não fosse Required).");
        }
    }
}