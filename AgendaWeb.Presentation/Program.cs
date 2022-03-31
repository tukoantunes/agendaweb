using AgendaWeb.Presentation;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configurar o projeto para o padr�o MVC
builder.Services.AddControllersWithViews();

//Habilitar o uso de sess�o no projeto (Session)
builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//Configura��o de inje��o de depend�ncia do projeto
DependencyInjection.Register(builder);

//habilitando o projeto para usar permiss�es de acesso (ticket) atraves de arquivos de cookie
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

//habilitando sess�es
app.UseSession();

//autentica��o e autoriza��o
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

//definir a p�gina inicial do projeto
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}"
);

app.Run();



