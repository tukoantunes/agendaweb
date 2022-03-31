using AgendaWeb.Infra.Data.Entities;
using AgendaWeb.Infra.Data.Interfaces;
using AgendaWeb.Infra.Reports.Interfaces;
using AgendaWeb.Infra.Reports.Services;
using AgendaWeb.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgendaWeb.Presentation.Controllers
{
    [Authorize]
    public class EventoController : Controller
    {
        //Página de cadastro de eventos
        public IActionResult Cadastro()
        {
            return View();
        }

        [HttpPost] //receber a ação de SUBMIT do formulário
        public IActionResult Cadastro(EventoCadastroModel model,
            [FromServices] IEventoRepository eventoRepository)
        {
            //verificar se todos os campos passaram nas regras de validação
            if (ModelState.IsValid)
            {
                try
                {
                    var evento = new Evento()
                    {
                        IdEvento = Guid.NewGuid(),
                        Nome = model.Nome,
                        Data = DateTime.Parse(model.Data),
                        Hora = TimeSpan.Parse(model.Hora),
                        Descricao = model.Descricao,
                        Prioridade = int.Parse(model.Prioridade),
                        Ativo = 1,
                        IdUsuario = ObterUsuarioAutenticado().IdUsuario
                    };

                    //gravar no banco de dados
                    eventoRepository.Add(evento);

                    //limpar os campos do formulário
                    ModelState.Clear();

                    TempData["MensagemSucesso"] = $"O evento '{evento.Nome}', foi cadastrado com sucesso!";
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Ocorreu um erro: {e.Message}";
                }
            }

            return View();
        }

        //Página de consulta de eventos
        public IActionResult Consulta()
        {
            return View();
        }

        //Método para processar o SUBMIT do formulário
        [HttpPost]
        public IActionResult Consulta(EventoConsultaModel model,
            [FromServices] IEventoRepository eventoRepository)
        {
            //verificar se a model passou nas regras de validação
            if (ModelState.IsValid)
            {
                try
                {
                    //convertendo para o tipo DateTime
                    var dataMin = DateTime.Parse(model.DataMin);
                    var dataMax = DateTime.Parse(model.DataMax);

                    //consultando os eventos no banco de dados
                    model.Eventos = eventoRepository.GetAll
                        (dataMin, dataMax, ObterUsuarioAutenticado().IdUsuario, int.Parse(model.Ativo));

                    if (model.Eventos != null && model.Eventos.Count > 0)
                        TempData["MensagemSucesso"] = $"A pesquisa obteve {model.Eventos.Count} registro(s).";
                    else
                        TempData["MensagemAlerta"] = "Nenhum registro foi encontrado para a pesquisa realizada.";
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Ocorreu um erro: {e.Message}";
                }
            }

            return View(model);
        }

        //ação de exclusão do evento
        public IActionResult AlterarStatus(Guid idEvento,
            [FromServices] IEventoRepository eventoRepository)
        {
            try
            {
                //obter os dados do usuário autenticao
                var usuario = ObterUsuarioAutenticado();

                //buscar o evento no banco de dados baseado no ID..
                var evento = eventoRepository.Get(idEvento);

                //verificar se o evento foi encontrado
                if (evento != null && evento.IdUsuario == usuario.IdUsuario)
                {
                    //alterar a flag Ativo
                    evento.Ativo = (evento.Ativo == 1 ? 0 : 1);
                    eventoRepository.Update(evento);

                    TempData["MensagemSucesso"] = $"Status do evento '{evento.Nome}', alterado com sucesso.";
                }
                else
                {
                    TempData["MensagemAlerta"] = "Evento não encontrado.";
                }
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = $"Ocorreu um erro: {e.Message}";
            }

            //redirecionamento
            return RedirectToAction("Consulta");
        }

        //Página de edição de eventos
        public IActionResult Edicao(Guid idEvento,
            [FromServices] IEventoRepository eventoRepository)
        {
            var model = new EventoEdicaoModel();

            try
            {
                //obter os dados do usuário autenticao
                var usuario = ObterUsuarioAutenticado();
                //buscar o evento no banco de dados baseado no ID..
                var evento = eventoRepository.Get(idEvento);

                //verificar se o evento foi encontrado
                if (evento != null && evento.IdUsuario == usuario.IdUsuario)
                {
                    model.IdEvento = evento.IdEvento;
                    model.Nome = evento.Nome;
                    model.Data = evento.Data.ToString("yyyy-MM-dd");
                    model.Hora = evento.Hora.ToString(@"hh\:mm");
                    model.Descricao = evento.Descricao;
                    model.Prioridade = evento.Prioridade.ToString();
                }
                else
                {
                    TempData["MensagemAlerta"] = "Evento não encontrado.";
                    return RedirectToAction("Consulta");
                }
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = $"Ocorreu um erro: {e.Message}";
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Edicao(EventoEdicaoModel model,
            [FromServices] IEventoRepository eventoRepository)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //buscar o evento no banco de dados
                    var evento = eventoRepository.Get(model.IdEvento);

                    evento.Nome = model.Nome;
                    evento.Data = DateTime.Parse(model.Data);
                    evento.Hora = TimeSpan.Parse(model.Hora);
                    evento.Descricao = model.Descricao;
                    evento.Prioridade = int.Parse(model.Prioridade);

                    //atualizando o evento
                    eventoRepository.Update(evento);

                    TempData["MensagemSucesso"] = $"Evento atualizado com sucesso.";
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Ocorreu um erro: {e.Message}";
                }
            }

            return View();
        }

        //método para acessar a página de relatórios
        public IActionResult Relatorio()
        {
            return View();
        }

        [HttpPost] //recebe a requisição do formulário
        public IActionResult Relatorio(EventoRelatorioModel model, [FromServices] IEventoRepository eventoRepository)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //convertendo para o tipo DateTime
                    var dataMin = DateTime.Parse(model.DataMin);
                    var dataMax = DateTime.Parse(model.DataMax);

                    //usuário autenticado
                    var usuarioModel = ObterUsuarioAutenticado();
                    var usuario = new Usuario
                    {
                        IdUsuario = usuarioModel.IdUsuario,
                        Nome = usuarioModel.Nome,
                        Email = usuarioModel.Email
                    };

                    //consultando os eventos no banco de dados
                    var eventos = eventoRepository.GetAll(dataMin, dataMax, usuario.IdUsuario, int.Parse(model.Ativo));

                    IEventoReportService eventoReportService = null;
                    var nomeArquivo = $"eventos_{DateTime.Now.ToString("ddMMyyyyHHmmss")}";
                    var tipoArquivo = string.Empty;

                    switch (model.Formato)
                    {
                        case "PDF":
                            //POLIMORFISMO..
                            eventoReportService = new EventoReportServicePdf();
                            nomeArquivo = $"{nomeArquivo}.pdf";
                            tipoArquivo = "application/pdf";
                            break;

                        case "EXCEL":
                            //POLIMORFISMO..
                            eventoReportService = new EventoReportServiceExcel();
                            nomeArquivo = $"{nomeArquivo}.xlsx";
                            tipoArquivo = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            break;
                    }

                    //gerar o relatório
                    var relatorio = eventoReportService.GerarRelatorio(usuario, eventos, dataMin, dataMax);

                    //download
                    return File(relatorio, tipoArquivo, nomeArquivo);
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Ocorreu um erro: {e.Message}";
                }
            }

            return View();
        }

        //método para retornar os dados do usuário da sessão
        private UsuarioModel ObterUsuarioAutenticado()
        {
            //ler o usuario da sessão
            var json = HttpContext.Session.GetString("usuario");
            //deserializando e retornando
            return JsonConvert.DeserializeObject<UsuarioModel>(json);
        }
    }
}



