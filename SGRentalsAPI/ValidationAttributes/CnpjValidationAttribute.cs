using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SGRentalsAPI.ValidationAttributes
{
    public class CnpjValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }

            string cnpj = value.ToString()!.Trim().Replace(".", "").Replace("-", "").Replace("/", "");

            if (cnpj.Length != 14 || new string(cnpj[0], 14) == cnpj)
            {
                return new ValidationResult("CNPJ inválido.");
            }

            int[] multiplier1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj.Substring(0, 12);
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier1[i];
            }
            int rest = sum % 11;
            int digit1 = (rest < 2) ? 0 : 11 - rest;

            if (int.Parse(cnpj.Substring(12, 1)) != digit1)
            {
                return new ValidationResult("CNPJ inválido.");
            }

            tempCnpj += digit1;
            sum = 0;
            for (int i = 0; i < 13; i++)
            {
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];
            }
            rest = sum % 11;
            int digit2 = (rest < 2) ? 0 : 11 - rest;

            if (int.Parse(cnpj.Substring(13, 1)) != digit2)
            {
                return new ValidationResult("CNPJ inválido.");
            }

            return ValidationResult.Success;
        }
    }
}
