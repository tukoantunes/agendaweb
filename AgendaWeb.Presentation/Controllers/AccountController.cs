using AgendaWeb.Infra.Data.Entities;
using AgendaWeb.Infra.Data.Interfaces;
using AgendaWeb.Presentation.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace AgendaWeb.Presentation.Controllers
{
    public class AccountController : Controller
    {
        //abre a página /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        //recebe o submit do formulário
        [HttpPost]
        public IActionResult Login(AccountLoginModel model,
            [FromServices] IUsuarioRepository usuarioRepository)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //consultar o usuário no banco de dados atraves do email e da senha
                    var usuario = usuarioRepository.Get(model.Email, model.Senha);

                    //verificar se o usuário foi encontrado!
                    if (usuario != null)
                    {
                        //fazendo a autenticação do usuário
                        AutenticarUsuario(usuario);

                        //redirecionando para a página /Home/Index
                        TempData["MensagemSucesso"] = $"Seja bem vindo {usuario.Nome}, seu acesso foi realizado com sucesso.";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["MensagemErro"] = "Acesso negado, usuário não identificado.";
                    }
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Ocorreu um erro: {e.Message}";
                }
            }

            return View();
        }

        //abre a página /Account/Register
        public IActionResult Register()
        {
            return View();
        }

        //recebe o submit do formulário
        [HttpPost]
        public IActionResult Register(AccountRegisterModel model,
            [FromServices] IUsuarioRepository usuarioRepository)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //verificando se o email informado já está cadastrado
                    if (usuarioRepository.Get(model.Email) != null)
                    {
                        TempData["MensagemErro"] = "O email informado já está cadastrado, por favor tente outro.";
                        return View();
                    }

                    //criando um objeto da classe Usuario
                    var usuario = new Usuario()
                    {
                        IdUsuario = Guid.NewGuid(),
                        Nome = model.Nome,
                        Email = model.Email,
                        Senha = model.Senha,
                        DataInclusao = DateTime.Now,
                        PrimeiroAcesso = 1
                    };

                    //gravando no banco de dados
                    usuarioRepository.Add(usuario);

                    //gerando mensagem de sucesso
                    TempData["MensagemSucesso"] = $"Parabéns {usuario.Nome}, seu cadastro foi realizado com sucesso!";

                    //limpar o formulário
                    ModelState.Clear();
                }
                catch (Exception e)
                {
                    TempData["MensagemErro"] = $"Ocorreu um erro: {e.Message}";
                }
            }

            return View();
        }

        //método para 'deslogar' o usuário
        public IActionResult Logout()
        {
            //apagando as permissões
            EncerrarSessao();

            //redirecionar de volta para a página de login
            return RedirectToAction("Login");
        }

        /// <summary>
        /// Método para criar a autorização de acesso do usuário
        /// </summary>
        /// <param name="idUsuario">ID do usuário autenticado</param>
        private void AutenticarUsuario(Usuario usuario)
        {
            //criando um ticket de acesso para o usuário autenticado
            //será gravado em um arquivo de cookie no navegador
            var identificacao = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, usuario.IdUsuario.ToString()) },
               CookieAuthenticationDefaults.AuthenticationScheme);

            var claimPrincipal = new ClaimsPrincipal(identificacao);
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);

            //armazenar os dados do usuário em sessão
            var model = new UsuarioModel
            {
                IdUsuario = usuario.IdUsuario,
                Nome = usuario.Nome,
                Email = usuario.Email,
                DataInclusao = usuario.DataInclusao.ToString("dd/MM/yyyy"),
                PrimeiroAcesso = usuario.PrimeiroAcesso,
                DataHoraAcesso = DateTime.Now.ToString("dd/MM/yyyy HH:mm")
            };

            //salvar os dados em sessão
            HttpContext.Session.SetString("usuario", JsonConvert.SerializeObject(model));
        }

        /// <summary>
        /// Método para remover as permissões e acesso do usuário
        /// </summary>
        private void EncerrarSessao()
        {
            //remover o cookie de autenticação do usuário
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            //apagar os dados gravados na sessão
            HttpContext.Session.Remove("usuario");
            HttpContext.Session.Clear();
        }
    }
}



