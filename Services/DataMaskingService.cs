using System;
using System.Text.RegularExpressions;

namespace ApostasCompulsivas.Services
{
    /// <summary>
    /// Serviço para mascarar dados sensíveis conforme LGPD
    /// </summary>
    public static class DataMaskingService
    {
        /// <summary>
        /// Mascara o nome do usuário, mantendo apenas a primeira letra
        /// </summary>
        public static string MascararNome(string nome)
        {
            if (string.IsNullOrEmpty(nome))
                return string.Empty;

            if (nome.Length <= 2)
                return nome[0] + new string('*', nome.Length - 1);

            return nome[0] + new string('*', nome.Length - 2) + nome[^1];
        }

        /// <summary>
        /// Mascara o email, mantendo apenas o primeiro caractere e o domínio
        /// </summary>
        public static string MascararEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !email.Contains('@'))
                return string.Empty;

            var partes = email.Split('@');
            if (partes.Length != 2)
                return string.Empty;

            var nomeUsuario = partes[0];
            var dominio = partes[1];

            if (nomeUsuario.Length <= 2)
                return nomeUsuario[0] + new string('*', nomeUsuario.Length - 1) + "@" + dominio;

            return nomeUsuario[0] + new string('*', nomeUsuario.Length - 2) + "@" + dominio;
        }

        /// <summary>
        /// Mascara o telefone, mantendo apenas os últimos 4 dígitos
        /// </summary>
        public static string MascararTelefone(string telefone)
        {
            if (string.IsNullOrEmpty(telefone))
                return string.Empty;

            // Remove caracteres não numéricos
            var apenasNumeros = Regex.Replace(telefone, @"[^\d]", "");
            
            if (apenasNumeros.Length < 4)
                return new string('*', apenasNumeros.Length);

            return new string('*', apenasNumeros.Length - 4) + apenasNumeros[^4..];
        }

        /// <summary>
        /// Mascara dados completos do usuário
        /// </summary>
        public static object MascararDadosUsuario(object usuario)
        {
            if (usuario == null)
                return null;

            var tipo = usuario.GetType();
            var propriedades = tipo.GetProperties();

            foreach (var prop in propriedades)
            {
                if (prop.PropertyType == typeof(string))
                {
                    var valor = prop.GetValue(usuario) as string;
                    if (!string.IsNullOrEmpty(valor))
                    {
                        string valorMascarado = prop.Name.ToLower() switch
                        {
                            "nome" => MascararNome(valor),
                            "email" => MascararEmail(valor),
                            "telefone" => MascararTelefone(valor),
                            _ => valor
                        };
                        prop.SetValue(usuario, valorMascarado);
                    }
                }
            }

            return usuario;
        }
    }
}
