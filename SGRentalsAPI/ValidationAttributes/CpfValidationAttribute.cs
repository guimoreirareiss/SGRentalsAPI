using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SGRentalsAPI.ValidationAttributes
{
    public class CpfValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }

            string cpf = value.ToString()!.Trim().Replace(".", "").Replace("-", "");

            if (cpf.Length != 11 || new string(cpf[0], 11) == cpf)
            {
                return new ValidationResult("CPF inválido.");
            }

            int[] multiplier1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];
            }
            int rest = sum % 11;
            int digit1 = (rest < 2) ? 0 : 11 - rest;

            if (int.Parse(cpf.Substring(9, 1)) != digit1)
            {
                return new ValidationResult("CPF inválido.");
            }

            tempCpf = tempCpf + digit1;
            sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];
            }
            rest = sum % 11;
            int digit2 = (rest < 2) ? 0 : 11 - rest;

            if (int.Parse(cpf.Substring(10, 1)) != digit2)
            {
                return new ValidationResult("CPF inválido.");
            }

            return ValidationResult.Success;
        }
    }
}   