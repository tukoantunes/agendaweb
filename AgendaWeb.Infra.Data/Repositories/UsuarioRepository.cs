using AgendaWeb.Infra.Data.Entities;
using AgendaWeb.Infra.Data.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgendaWeb.Infra.Data.Repositories
{
    /// <summary>
    /// Classe de repositório de dados para Usuário
    /// </summary>
    public class UsuarioRepository : IUsuarioRepository
    {
        //atributo
        private readonly string _connectionString;

        //Construtor para entrada de argumentos
        public UsuarioRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Usuario entity)
        {
            var query = @"
                INSERT INTO USUARIO(
                    IDUSUARIO,
                    NOME,
                    EMAIL,
                    SENHA,
                    DATAINCLUSAO,
                    PRIMEIROACESSO)
                VALUES(
                    @IdUsuario,
                    @Nome,
                    @Email,
                    CONVERT(VARCHAR(32), HASHBYTES('MD5', @Senha), 2),
                    @DataInclusao,
                    @PrimeiroAcesso)
            ";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public void Update(Usuario entity)
        {
            var query = @"
                UPDATE USUARIO
                SET
                    NOME = @Nome,
                    EMAIL = @Email,
                    PRIMEIROACESSO = @PrimeiroAcesso
                WHERE
                    IDUSUARIO = @IdUsuario
            ";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public void Delete(Usuario entity)
        {
            var query = @"
                DELETE FROM USUARIO
                WHERE IDUSUARIO = @IdUsuario
            ";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public List<Usuario> GetAll()
        {
            var query = @"
                SELECT * FROM USUARIO
                ORDER BY NOME ASC
            ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Usuario>(query)
                    .ToList();
            }
        }

        public Usuario Get(Guid id)
        {
            var query = @"
                SELECT * FROM USUARIO
                WHERE 
                    IDUSUARIO = @id
            ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Usuario>(query, new { id })
                    .FirstOrDefault();
            }
        }

        public Usuario Get(string email)
        {
            var query = @"
                SELECT * FROM USUARIO
                WHERE 
                    EMAIL = @email
            ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Usuario>(query, new { email })
                    .FirstOrDefault();
            }
        }

        public Usuario Get(string email, string senha)
        {
            var query = @"
                SELECT * FROM USUARIO
                WHERE 
                    EMAIL = @email 
                AND 
                    SENHA = CONVERT(VARCHAR(32), HASHBYTES('MD5', @senha), 2)
            ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Usuario>(query, new { email, senha })
                    .FirstOrDefault();
            }
        }
    }
}



