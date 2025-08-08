using Xunit;
using SGRentalsAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;

namespace SGRentalsAPI.Tests
{
    public class EnderecoTests
    {
        [Fact]
        public void Deve_Criar_Endereco_Com_Campos_Obrigatorios()
        {
            
            var endereco = new Endereco
            {
                Logradouro = "Avenida Beira Mar Norte",
                Numero = "1566",
                Complemento = "Apto 101",
                Bairro = "Centro",
                Cidade = "Florianópolis",
                Estado = "SC",
                Cep = "01000000"
            };

            // Act
            var validationContext = new ValidationContext(endereco, serviceProvider: null, items: null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(endereco, validationContext, validationResults, validateAllProperties: true);

            // Assert
            Assert.True(isValid, $"Validação do endereço falhou: {string.Join(", ", validationResults.Select(r => r.ErrorMessage))}");
            Assert.Equal("Avenida Beira Mar Norte", endereco.Logradouro);
            Assert.Equal("1566", endereco.Numero);
            Assert.Equal("Apto 101", endereco.Complemento);
            Assert.Equal("Centro", endereco.Bairro);
            Assert.Equal("Florianópolis", endereco.Cidade);
            Assert.Equal("SC", endereco.Estado);
            Assert.Equal("01000000", endereco.Cep);
        }

        [Fact]
        public void Nao_Deve_Criar_Endereco_Com_Campos_Vazios_Obrigatorios()
        {
            // Arrange
            var endereco = new Endereco
            {
                Logradouro = "",
                Numero = null!,
                Complemento = "Sala 5",
                Bairro = "",
                Cidade = "Rio de Janeiro",
                Estado = "",
                Cep = "20000000"
            };

            var validationContext = new ValidationContext(endereco, serviceProvider: null, items: null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(endereco, validationContext, validationResults, validateAllProperties: true);

            Assert.False(isValid, "Validação do endereço deveria ter falhado para campos vazios/nulos.");
            Assert.Contains("Logradouro é obrigatório", validationResults.Select(r => r.ErrorMessage));
            Assert.Contains("Número é obrigatório", validationResults.Select(r => r.ErrorMessage));
            Assert.Contains("Bairro é obrigatório", validationResults.Select(r => r.ErrorMessage));
            Assert.Contains("Estado é obrigatório", validationResults.Select(r => r.ErrorMessage));
        }

        [Fact]
        public void Nao_Deve_Criar_Endereco_Com_Todos_Campos_Obrigatorios_Ausentes()
        {
            // Arrange
            var endereco = new Endereco
            {
                Logradouro = null!,
                Numero = null!,
                Complemento = null,
                Bairro = null!,
                Cidade = null!,
                Estado = null!,
                Cep = null!
            };

            var validationContext = new ValidationContext(endereco, serviceProvider: null, items: null);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(endereco, validationContext, validationResults, validateAllProperties: true);

            Assert.False(isValid, "Validação do endereço deveria ter falhado para todos os campos obrigatórios ausentes.");
            Assert.Contains("Logradouro é obrigatório", validationResults.Select(r => r.ErrorMessage));
            Assert.Contains("Número é obrigatório", validationResults.Select(r => r.ErrorMessage));
            Assert.Contains("Bairro é obrigatório", validationResults.Select(r => r.ErrorMessage));
            Assert.Contains("Cidade é obrigatória", validationResults.Select(r => r.ErrorMessage));
            Assert.Contains("Estado é obrigatório", validationResults.Select(r => r.ErrorMessage));
            Assert.Contains("Cep é obrigatório", validationResults.Select(r => r.ErrorMessage));
        }
    }
}