using AgendaWeb.Infra.Data.DTOs;
using AgendaWeb.Infra.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaWeb.Infra.Data.Interfaces
{
    /// <summary>
    /// Interface para operações de repositório de Evento
    /// </summary>
    public interface IEventoRepository : IBaseRepository<Evento>
    {
        /// <summary>
        /// Consulta de eventos por periodo de datas e usuário
        /// </summary>
        List<Evento> GetAll(DateTime dataMin, DateTime dataMax, Guid idUsuario, int ativo);

        /// <summary>
        /// Consulta de eventos agrupado por prioridade, filtrando por periodo de datas e usuário
        /// </summary>
        List<ConsultaPrioridadeDTO> GroupByPrioridade(DateTime dataMin, DateTime dataMax, Guid idUsuario);

        /// <summary>
        /// Consulta de eventos agrupado por ativo/inativo, filtrando por periodo de datas e usuário
        /// </summary>
        List<ConsultaAtivoInativoDTO> GroupByAtivo(DateTime dataMin, DateTime dataMax, Guid idUsuario);
    }
}



