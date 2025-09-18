using System;
using System.Text.RegularExpressions;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Serviço centralizado para validações de entrada
    /// </summary>
    public static class ValidationService
    {
        /// <summary>
        /// Valida se o nome é válido
        /// </summary>
        public static ValidationResult ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return new ValidationResult(false, "Nome não pode ser vazio");

            if (nome.Length < 2)
                return new ValidationResult(false, "Nome deve ter pelo menos 2 caracteres");

            if (nome.Length > 100)
                return new ValidationResult(false, "Nome não pode ter mais de 100 caracteres");

            if (!Regex.IsMatch(nome, @"^[a-zA-ZÀ-ÿ\s]+$"))
                return new ValidationResult(false, "Nome deve conter apenas letras e espaços");

            return new ValidationResult(true, string.Empty);
        }

        /// <summary>
        /// Valida se o email é válido
        /// </summary>
        public static ValidationResult ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return new ValidationResult(false, "Email não pode ser vazio");

            if (email.Length > 255)
                return new ValidationResult(false, "Email não pode ter mais de 255 caracteres");

            var emailRegex = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, emailRegex))
                return new ValidationResult(false, "Email deve ter um formato válido");

            return new ValidationResult(true, string.Empty);
        }

        /// <summary>
        /// Valida se o telefone é válido
        /// </summary>
        public static ValidationResult ValidarTelefone(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                return new ValidationResult(true, string.Empty); // Telefone é opcional

            // Remove caracteres não numéricos
            var apenasNumeros = Regex.Replace(telefone, @"[^\d]", "");

            if (apenasNumeros.Length < 10)
                return new ValidationResult(false, "Telefone deve ter pelo menos 10 dígitos");

            if (apenasNumeros.Length > 15)
                return new ValidationResult(false, "Telefone não pode ter mais de 15 dígitos");

            return new ValidationResult(true, string.Empty);
        }

        /// <summary>
        /// Valida se o valor monetário é válido
        /// </summary>
        public static ValidationResult ValidarValorMonetario(decimal valor, string campo = "Valor")
        {
            if (valor < 0)
                return new ValidationResult(false, $"{campo} não pode ser negativo");

            if (valor > 999999.99m)
                return new ValidationResult(false, $"{campo} não pode ser maior que R$ 999.999,99");

            return new ValidationResult(true, string.Empty);
        }

        /// <summary>
        /// Valida se o ID é válido
        /// </summary>
        public static ValidationResult ValidarId(int id, string campo = "ID")
        {
            if (id <= 0)
                return new ValidationResult(false, $"{campo} deve ser maior que zero");

            return new ValidationResult(true, string.Empty);
        }

        /// <summary>
        /// Valida se o texto não é muito longo
        /// </summary>
        public static ValidationResult ValidarTexto(string texto, string campo, int maxLength = 255)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return new ValidationResult(false, $"{campo} não pode ser vazio");

            if (texto.Length > maxLength)
                return new ValidationResult(false, $"{campo} não pode ter mais de {maxLength} caracteres");

            return new ValidationResult(true, string.Empty);
        }
    }

    /// <summary>
    /// Resultado de uma validação
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; }
        public string ErrorMessage { get; }

        public ValidationResult(bool isValid, string errorMessage)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }
    }
}
