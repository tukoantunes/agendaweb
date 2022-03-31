using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaWeb.Infra.Data.DTOs
{
    /// <summary>
    /// DTO para captura dos dados da consulta de evento agrupado por prioridade
    /// </summary>
    public class ConsultaPrioridadeDTO
    {
        public string Prioridade { get; set; }
        public int Quantidade { get; set; }
    }
}



