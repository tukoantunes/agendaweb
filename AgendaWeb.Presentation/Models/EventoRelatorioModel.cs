using System.ComponentModel.DataAnnotations;

namespace AgendaWeb.Presentation.Models
{
    /// <summary>
    /// Classe de modelo de dados para a página de relatório de eventos
    /// </summary>
    public class EventoRelatorioModel
    {
        [Required(ErrorMessage = "Por favor, informe a data de início.")]
        public string DataMin { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data de término.")]
        public string DataMax { get; set; }

        [Required(ErrorMessage = "Por favor, selecione uma opção.")]
        public string Ativo { get; set; }

        [Required(ErrorMessage = "Por favor, selecione uma opção.")]
        public string Formato { get; set; }
    }
}



