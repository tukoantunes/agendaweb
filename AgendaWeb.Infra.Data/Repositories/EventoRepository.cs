using AgendaWeb.Infra.Data.DTOs;
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
    /// Classe de repositório de dados para Evento
    /// </summary>
    public class EventoRepository : IEventoRepository
    {
        //atributo
        private readonly string _connectionString;

        //construtor com entrada de argumentos
        public EventoRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Add(Evento entity)
        {
            var query = @"
                    INSERT INTO EVENTO(
                        IDEVENTO,
                        NOME,
                        DATA,
                        HORA,
                        PRIORIDADE,
                        DESCRICAO,
                        ATIVO,
                        IDUSUARIO)
                    VALUES(
                        @IdEvento,
                        @Nome,
                        @Data,
                        @Hora,
                        @Prioridade,
                        @Descricao,
                        @Ativo,
                        @IdUsuario)
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public void Update(Evento entity)
        {
            var query = @"
                    UPDATE EVENTO
                    SET
                        NOME = @Nome,
                        DATA = @Data,
                        HORA = @Hora,
                        PRIORIDADE = @Prioridade,
                        DESCRICAO = @Descricao,
                        ATIVO = @Ativo
                    WHERE
                        IDEVENTO = @IdEvento
                    AND
                        IDUSUARIO = @IdUsuario
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public void Delete(Evento entity)
        {
            var query = @"
                    DELETE FROM EVENTO
                    WHERE
                        IDEVENTO = @IdEvento
                    AND
                        IDUSUARIO = @IdUsuario
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Execute(query, entity);
            }
        }

        public List<Evento> GetAll()
        {
            var query = @"
                    SELECT * FROM EVENTO
                    ORDER BY DATA
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Evento>(query)
                    .ToList();
            }
        }

        public Evento Get(Guid id)
        {
            var query = @"
                    SELECT * FROM EVENTO
                    WHERE IDEVENTO = @id
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Evento>(query, new { id })
                    .FirstOrDefault();
            }
        }

        public List<Evento> GetAll(DateTime dataMin, DateTime dataMax, Guid idUsuario, int ativo)
        {
            var query = @"
                    SELECT * FROM EVENTO
                    WHERE
                        DATA BETWEEN @dataMin AND @dataMax
                    AND
                        IDUSUARIO = @idUsuario
                    AND
                        Ativo = @ativo
                    ORDER BY DATA
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<Evento>(query, new { dataMin, dataMax, idUsuario, ativo })
                    .ToList();
            }
        }

        public List<ConsultaPrioridadeDTO> GroupByPrioridade(DateTime dataMin, DateTime dataMax, Guid idUsuario)
        {
            var query = @"
                    SELECT 	
	                    CASE 
		                    WHEN PRIORIDADE = 0 THEN 'ALTA'
		                    WHEN PRIORIDADE = 1 THEN 'MÉDIA'
		                    WHEN PRIORIDADE = 2 THEN 'BAIXA'
	                    END AS PRIORIDADE,
	                    COUNT(*) AS QUANTIDADE
                    FROM EVENTO
                    WHERE
	                    DATA BETWEEN @dataMin AND @dataMax
                    AND
	                    IDUSUARIO = @idUsuario
                    GROUP BY PRIORIDADE
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<ConsultaPrioridadeDTO>(query, new { dataMin, dataMax, idUsuario })
                    .ToList();
            }
        }

        public List<ConsultaAtivoInativoDTO> GroupByAtivo(DateTime dataMin, DateTime dataMax, Guid idUsuario)
        {
            var query = @"
                    SELECT 	
	                    CASE 
		                    WHEN ATIVO = 0 THEN 'INATIVOS'
		                    WHEN ATIVO = 1 THEN 'ATIVOS'
	                    END AS ATIVO,
	                    COUNT(*) AS QUANTIDADE
                    FROM EVENTO
                    WHERE
	                    DATA BETWEEN @dataMin AND @dataMax
                    AND
	                    IDUSUARIO = @idUsuario
                    GROUP BY ATIVO
                ";

            using (var connection = new SqlConnection(_connectionString))
            {
                return connection
                    .Query<ConsultaAtivoInativoDTO>(query, new { dataMin, dataMax, idUsuario })
                    .ToList();
            }
        }
    }
}



