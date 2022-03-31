using System.ComponentModel.DataAnnotations;

namespace AgendaWeb.Presentation.Models.Validations
{
    /// <summary>
    /// Classe de validação para campos de senha
    /// </summary>
    public class PasswordValidation : ValidationAttribute
    {
        /// <summary>
        /// Método para implementar a regra de validação
        /// </summary>
        /// <param name="value">Representa o valor do campo que será validado</param>
        /// <returns>True/False dependendo do resultado da validação</returns>
        public override bool IsValid(object? value)
        {
            //verificar se o valor recebido não é vazio
            if (value != null)
            {
                //converter o valor para string
                var senha = value.ToString();

                //verificar o conteudo da senha
                return senha.Any(s => char.IsLower(s)) //pelo menos 1 letra minúscula
                    && senha.Any(s => char.IsUpper(s)) //pelo menos 1 letra maiúscula
                    && senha.Any(s => char.IsDigit(s)) //pelo menos 1 número
                    && (
                        senha.Contains("@") ||
                        senha.Contains("$") ||
                        senha.Contains("#") ||
                        senha.Contains("%")
                    );
            }

            return false;
        }
    }
}



