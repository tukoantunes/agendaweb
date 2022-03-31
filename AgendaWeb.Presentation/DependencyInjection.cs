using AgendaWeb.Infra.Data.Interfaces;
using AgendaWeb.Infra.Data.Repositories;

namespace AgendaWeb.Presentation
{
    /// <summary>
    /// Classe para configuração da injeção de dependência do projeto
    /// </summary>
    public class DependencyInjection
    {
        /// <summary>
        /// Método para registrar e configurar as dependências
        /// </summary>
        public static void Register(WebApplicationBuilder builder)
        {
            #region Capturar a connectionstring mapeada no arquivo appsettings.json

            var connectionString = builder.Configuration.GetConnectionString("AgendaWebDev");

            #endregion

            #region Configurando a injeção de dependência para o repositorio

            builder.Services.AddTransient<IUsuarioRepository>
                (map => new UsuarioRepository(connectionString));

            builder.Services.AddTransient<IEventoRepository>
               (map => new EventoRepository(connectionString));

            #endregion

        }
    }
}



