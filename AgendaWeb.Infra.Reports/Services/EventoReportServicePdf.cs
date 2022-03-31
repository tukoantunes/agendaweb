using AgendaWeb.Infra.Data.Entities;
using AgendaWeb.Infra.Reports.Interfaces;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaWeb.Infra.Reports.Services
{
    /// <summary>
    /// Classe para geração de relatório de eventos em formato PDF
    /// </summary>
    public class EventoReportServicePdf : IEventoReportService
    {
        public byte[] GerarRelatorio(Usuario usuario, List<Evento> eventos, DateTime dataMin, DateTime dataMax)
        {
            //criando o relatorio em memória
            var memoryStream = new MemoryStream();
            var pdf = new PdfDocument(new PdfWriter(memoryStream));

            //escrevendo o conteudo do relatorio
            using (var document = new Document(pdf))
            {
                document.Add(new Paragraph("Relatório de eventos"));

                document.Add(new Paragraph($"Data de início: {dataMin.ToString("dd/MM/yyyy")}"));
                document.Add(new Paragraph($"Data de fim: {dataMax.ToString("dd/MM/yyyy")}"));
                document.Add(new Paragraph($"Nome do usuário: {usuario.Nome}"));

                document.Add(new Paragraph("\n")); //quebra de linha

                var table = new Table(5);

                table.AddHeaderCell("Nome do evento");
                table.AddHeaderCell("Data");
                table.AddHeaderCell("Hora");
                table.AddHeaderCell("Prioridade");
                table.AddHeaderCell("Descrição");

                foreach (var item in eventos)
                {
                    table.AddCell(item.Nome);
                    table.AddCell(item.Data.ToString("dd/MM/yyyy"));
                    table.AddCell(item.Hora.ToString(@"hh\:mm"));
                    table.AddCell(item.Prioridade == 0 ? "ALTA" : item.Prioridade == 1 ? "MÉDIA" : "BAIXA");
                    table.AddCell(item.Descricao);
                }

                document.Add(table);
            }

            return memoryStream.ToArray();
        }
    }
}



