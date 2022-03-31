using AgendaWeb.Infra.Data.Entities;
using AgendaWeb.Infra.Reports.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaWeb.Infra.Reports.Services
{
    /// <summary>
    /// Classe para geração de relatório de eventos em formato EXCEL
    /// </summary>
    public class EventoReportServiceExcel : IEventoReportService
    {
        public byte[] GerarRelatorio(Usuario usuario, List<Evento> eventos, DateTime dataMin, DateTime dataMax)
        {
            //definindo o uso 'free' da biblioteca
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            //inicializando o arquivo em memória
            using (var excelPackage = new ExcelPackage())
            {
                //criando a planilha..
                var planilha = excelPackage.Workbook.Worksheets.Add("Eventos");

                //escrevendo o conteudo da planilha
                planilha.Cells["A1"].Value = "Relatório de eventos";

                planilha.Cells["A3"].Value = "Data de início:";
                planilha.Cells["B3"].Value = dataMin.ToString("dd/MM/yyyy");

                planilha.Cells["A4"].Value = "Data de Fim:";
                planilha.Cells["B4"].Value = dataMax.ToString("dd/MM/yyyy");

                planilha.Cells["A5"].Value = "Nome do usuário:";
                planilha.Cells["B5"].Value = usuario.Nome;

                planilha.Cells["A7"].Value = "ID do Evento";
                planilha.Cells["B7"].Value = "Nome do Evento";
                planilha.Cells["C7"].Value = "Data";
                planilha.Cells["D7"].Value = "Hora";
                planilha.Cells["E7"].Value = "Prioridade";
                planilha.Cells["F7"].Value = "Descrição";

                var linha = 8;
                foreach (var item in eventos)
                {
                    planilha.Cells[$"A{linha}"].Value = item.IdEvento.ToString();
                    planilha.Cells[$"B{linha}"].Value = item.Nome;
                    planilha.Cells[$"C{linha}"].Value = item.Data.ToString("dd/MM/yyyy");
                    planilha.Cells[$"D{linha}"].Value = item.Hora.ToString(@"hh\:mm");
                    planilha.Cells[$"E{linha}"].Value = item.Prioridade == 0 ? "ALTA" : item.Prioridade == 1 ? "MÉDIA" : "BAIXA";
                    planilha.Cells[$"F{linha}"].Value = item.Descricao;

                    linha++;
                }

                //ajustar a largura das colunas
                planilha.Cells["A:F"].AutoFitColumns();

                //retornando a planilha em formato de arquivo..
                return excelPackage.GetAsByteArray();
            }
        }
    }
}



