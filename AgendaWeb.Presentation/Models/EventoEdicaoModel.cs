using System.ComponentModel.DataAnnotations;

namespace AgendaWeb.Presentation.Models
{
    public class EventoEdicaoModel
    {
        public Guid IdEvento { get; set; }

        [Required(ErrorMessage = "Por favor, informe o nome do evento.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data do evento.")]
        public string Data { get; set; }

        [Required(ErrorMessage = "Por favor, informe a hora do evento.")]
        public string Hora { get; set; }

        [Required(ErrorMessage = "Por favor, informe a prioridade do evento.")]
        public string Prioridade { get; set; }

        [Required(ErrorMessage = "Por favor, informe a descrição do evento.")]
        public string Descricao { get; set; }
    }
}



