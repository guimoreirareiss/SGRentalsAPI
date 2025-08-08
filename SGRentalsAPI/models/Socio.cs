using System.ComponentModel.DataAnnotations;
using SGRentalsAPI.ValidationAttributes; // Referência ao namespace dos validadores

namespace SGRentalsAPI.Models
{
    // Classe que representa o modelo de dados de um Sócio.
    public class Socio
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Nome do sócio é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF do sócio é obrigatório.")]
        [CpfValidation(ErrorMessage = "CPF inválido.")]
        [StringLength(14, MinimumLength = 11, ErrorMessage = "O CPF deve ter entre 11 e 14 caracteres.")]
        public string Cpf { get; set; } = string.Empty;

        public int? EmpresaId { get; set; }
        public Empresa? Empresa { get; set; }
    }
}