using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SGRentalsAPI.ValidationAttributes; // Referência ao namespace dos validadores

namespace SGRentalsAPI.Models
{
    // Classe que representa o modelo de dados de uma Empresa.
    public class Empresa
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "A Razão Social é obrigatória.")]
        [StringLength(200, ErrorMessage = "A Razão Social não pode exceder 200 caracteres.")]
        public string RazaoSocial { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "O Nome Fantasia não pode exceder 200 caracteres.")]
        public string? NomeFantasia { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [CnpjValidation(ErrorMessage = "CNPJ inválido.")]
        [StringLength(18, MinimumLength = 14, ErrorMessage = "O CNPJ deve ter entre 14 e 18 caracteres.")]
        public string Cnpj { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Tipo de Empresa é obrigatório.")]
        [StringLength(50, ErrorMessage = "O Tipo de Empresa não pode exceder 50 caracteres.")]
        public string TipoEmpresa { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public int? EnderecoId { get; set; }
        public Endereco? Endereco { get; set; }

        // Coleções de navegação para EF Core.
        public ICollection<Socio> Socios { get; set; } = new List<Socio>();
        public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    }
}