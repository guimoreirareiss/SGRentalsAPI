using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SGRentalsAPI.ValidationAttributes; // Referência ao namespace dos validadores

namespace SGRentalsAPI.Models
{
    // Classe que representa o modelo de dados de um Usuário.
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O Nome do usuário é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Nome não pode exceder 100 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de Email inválido.")]
        [StringLength(100, ErrorMessage = "O Email não pode exceder 100 caracteres.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Senha Hash é obrigatória.")]
        public string SenhaHash { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Senha Salt é obrigatória.")]
        public string SenhaSalt { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [CpfValidation(ErrorMessage = "CPF inválido.")]
        [StringLength(14, MinimumLength = 11, ErrorMessage = "O CPF deve ter entre 11 e 14 caracteres.")]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "O Perfil é obrigatório.")]
        public PerfilUsuario Perfil { get; set; }

        public bool IsActive { get; set; } = true;

        public int? EmpresaId { get; set; }
        public Empresa? Empresa { get; set; }
    }
}