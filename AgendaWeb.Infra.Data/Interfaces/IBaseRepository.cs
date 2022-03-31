using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaWeb.Infra.Data.Interfaces
{
    /// <summary>
    /// Interface de repositório genérica
    /// </summary>
    /// <typeparam name="TEntity">Tipo genérico para as classes de entidade</typeparam>
    public interface IBaseRepository<TEntity>
        where TEntity : class
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        List<TEntity> GetAll();
        TEntity Get(Guid id);
    }
}



