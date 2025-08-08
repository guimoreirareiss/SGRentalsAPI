using Xunit;
using SGRentalsAPI.Models;

namespace SGRentalsAPI.Tests
{
    public class EmpresaTests
    {
        [Fact]
        public void Deve_Criar_Empresa_Com_Tipo_E_Validar_Valor()
        {
            var empresa = new Empresa
            {
                RazaoSocial = "Empresa de Teste Ltda", // Adicionado
                Cnpj = "11222333000144", // Adicionado
                TipoEmpresa = "MEI",
                EnderecoId = 1 // Adicionado
            };

            Assert.Equal("MEI", empresa.TipoEmpresa);
        }
    }
}