using Xunit;
using SGRentalsAPI.Models;
using System;
using System.Linq;

namespace SGRentalsAPI.Tests
{
    public class PerfilUsuarioTestes
    {
        [Fact]
        public void PerfilUsuario_Deve_Conter_Todos_Os_Perfis_Esperados()
        {
            // Obtém todos os valores definidos no enum PerfilUsuario
            var allProfiles = Enum.GetValues<PerfilUsuario>().ToList();

            // Verifica se cada perfil esperado está presente no enum
            Assert.Contains(PerfilUsuario.Agencia, allProfiles);
            Assert.Contains(PerfilUsuario.Parceiro, allProfiles);
            Assert.Contains(PerfilUsuario.Colaborador, allProfiles);
            Assert.Contains(PerfilUsuario.Administrador, allProfiles);
            
            // Verificar se não há perfias inesperados, o número é quantidade esperada
            Assert.Equal(4, allProfiles.Count);
        }
    }
}