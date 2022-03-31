using AgendaWeb.Presentation;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configurar o projeto para o padrão MVC
builder.Services.AddControllersWithViews();

//Habilitar o uso de sessão no projeto (Session)
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Configuração de injeção de dependência do projeto
DependencyInjection.Register(builder);

//habilitando o projeto para usar permissões de acesso (ticket) atraves de arquivos de cookie
builder.Services.Configure<CookiePolicyOptions>
    (options => { options.MinimumSameSitePolicy = SameSiteMode.None; });

builder.Services.AddAuthentication
    (CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

//habilitando sessões
app.UseSession();

//autenticação e autorização
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

//definir a página inicial do projeto
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}"
);

app.Run();



