using AgendaWeb.Infra.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaWeb.Infra.Reports.Interfaces
{
    /// <summary>
    /// Interface para definição dos métodos utilizados para geração
    /// dos relatorios de dados de evento
    /// </summary>
    public interface IEventoReportService
    {
        /// <summary>
        /// Método abstrato para implementação de relatórios
        /// </summary>
        /// <param name="usuario">Dados do usuário logado que gerou o relatório</param>
        /// <param name="eventos">Lista de eventos que será exibida no relatório</param>
        /// <param name="dataMin">Data de início do filtro de pesquisa</param>
        /// <param name="dataMax">Data de fim do filtro de pesquisa</param>
        /// <returns></returns>
        byte[] GerarRelatorio(
            Usuario usuario,
            List<Evento> eventos,
            DateTime dataMin,
            DateTime dataMax);
    }
}



