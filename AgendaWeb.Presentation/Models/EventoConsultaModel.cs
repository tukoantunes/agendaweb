using AgendaWeb.Infra.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace AgendaWeb.Presentation.Models
{
    /// <summary>
    /// Classe de modelo de dados para a página de consulta de eventos
    /// </summary>
    public class EventoConsultaModel
    {
        [Required(ErrorMessage = "Por favor, informe a data de início.")]
        public string DataMin { get; set; }

        [Required(ErrorMessage = "Por favor, informe a data de término.")]
        public string DataMax { get; set; }

        [Required(ErrorMessage = "Por favor, selecione uma opção.")]
        public string Ativo { get; set; }

        //propriedade para armazenar o resultado da consulta
        public List<Evento>? Eventos { get; set; }
    }
}



