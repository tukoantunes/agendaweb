using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaWeb.Infra.Data.Entities
{
    /// <summary>
    /// Classe de entidade
    /// </summary>
    public class Evento
    {
        #region Propriedades

        public Guid IdEvento { get; set; }
        public string Nome { get; set; }
        public DateTime Data { get; set; }
        public TimeSpan Hora { get; set; }
        public int Prioridade { get; set; }
        public string Descricao { get; set; }
        public int Ativo { get; set; }
        public Guid IdUsuario { get; set; }

        #endregion

        #region Associações

        public Usuario Usuario { get; set; }

        #endregion
    }
}
