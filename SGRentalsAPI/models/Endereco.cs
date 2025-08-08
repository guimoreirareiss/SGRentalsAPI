using System.ComponentModel.DataAnnotations;

namespace SGRentalsAPI.Models
{
    // Classe que representa o modelo de dados de um Endereço.
    public class Endereco
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Logradouro é obrigatório.")]
        [StringLength(200, ErrorMessage = "O Logradouro não pode exceder 200 caracteres.")]
        public string Logradouro { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Número é obrigatório.")]
        [StringLength(20, ErrorMessage = "O Número não pode exceder 20 caracteres.")]
        public string Numero { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "O Complemento não pode exceder 100 caracteres.")]
        public string? Complemento { get; set; }

        [Required(ErrorMessage = "O Bairro é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Bairro não pode exceder 100 caracteres.")]
        public string Bairro { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Cidade é obrigatória.")]
        [StringLength(100, ErrorMessage = "A Cidade não pode exceder 100 caracteres.")]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Estado é obrigatório.")]
        [StringLength(2, ErrorMessage = "O Estado deve ter 2 caracteres (UF).")]
        public string Estado { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CEP é obrigatório.")]
        [StringLength(9, MinimumLength = 8, ErrorMessage = "O CEP deve ter entre 8 e 9 caracteres.")]
        public string Cep { get; set; } = string.Empty;
    }
}