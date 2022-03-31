using AgendaWeb.Infra.Data.Interfaces;
using AgendaWeb.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgendaWeb.Presentation.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index([FromServices] IEventoRepository eventoRepository)
        {
            //criando um objeto da classe model
            var model = new EventoDashboardModel();

            try
            {
                //usuário autenticado
                var usuario = ObterUsuarioAutenticado();

                //mes vigente
                var mes = DateTime.Now.Month;
                var ano = DateTime.Now.Year;

                //primeiro dia do mes vigente
                var dataMin = new DateTime(ano, mes, 1);
                var dataMax = new DateTime(ano, mes, DateTime.DaysInMonth(ano, mes));

                //consultar os dados no banco e popular a model
                model.DataMin = dataMin.ToString("yyyy-MM-dd");
                model.DataMax = dataMax.ToString("yyyy-MM-dd");

                model.ConsultaPrioridade = eventoRepository.GroupByPrioridade(dataMin, dataMax, usuario.IdUsuario);
                model.ConsultaAtivoInativo = eventoRepository.GroupByAtivo(dataMin, dataMax, usuario.IdUsuario);
            }
            catch (Exception e)
            {
                TempData["MensagemErro"] = e.Message;
            }

            return View(model);
        }

        [HttpPost] //recebe o SUBMIT do formulário
        public IActionResult Index(EventoDashboardModel model, [FromServices] IEventoRepository eventoRepository)
        {
            //verificar se todos os campos passaram nas regras de validação
            if (ModelState.IsValid)
            {
                try
                {
                    var dataMin = DateTime.Parse(model.DataMin);
                    var dataMax = DateTime.Parse(model.DataMax);
                    var usuario = ObterUsuarioAutenticado();

                    model.ConsultaPrioridade = eventoRepository.GroupByPrioridade(dataMin, dataMax, usuario.IdUsuario);
                    model.ConsultaAtivoInativo = eventoRepository.GroupByAtivo(dataMin, dataMax, usuario.IdUsuario);
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = e.Message;
                }
            }

            return View(model);
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



