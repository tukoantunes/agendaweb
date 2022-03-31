namespace AgendaWeb.Presentation.Models
{
    /// <summary>
    /// Classe de modelo de dados para as informações do usuário
    /// autenticado que são gravadas em sessão
    /// </summary>
    public class UsuarioModel
    {
        public Guid IdUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string DataInclusao { get; set; }
        public int PrimeiroAcesso { get; set; }
        public string DataHoraAcesso { get; set; }
    }
}



