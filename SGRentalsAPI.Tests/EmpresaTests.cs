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
                RazaoSocial = "Empresa de Teste Ltda",
                Cnpj = "11222333000144",
                TipoEmpresa = "MEI",
                EnderecoId = 1
            };

            Assert.Equal("MEI", empresa.TipoEmpresa);
        }
    }
}
