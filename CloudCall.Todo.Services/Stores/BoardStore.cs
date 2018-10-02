using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using CloudCall.Todo.DAL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CloudCall.Todo.DAL.Models;
using Dapper;

namespace CloudCall.Todo.Services.Stores
{
    public class BoardStore : IStore<Board>
    {
        private readonly string _connectionString;
        private readonly ILogger<ListStore> _logger;

        public BoardStore(IConfiguration configuration, ILogger<ListStore> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }


        public int Add(Board entity, int userId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    int boardId = connection.Insert(entity).Value;
                    return connection.Insert(new ApplicationUserBoard {BoardId = boardId, ApplicationUserId = userId}).Value;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Board Add error");
                throw;
            }
        }

        public Board Get(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    return connection.Get<Board>(id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Board Get error");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Delete(id);
                    connection.DeleteList<ApplicationUserBoard>(new { BoardId = id});
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Board Delete error");
                throw;
            }
        }

        public void Update(Board entity)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Update(entity);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Board Update error");
                throw;
            }
        }

        public IEnumerable<Board> GetList(int userId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var resultList = new List<Board>();
                    var linksApplicationUserBoards = connection.GetList<ApplicationUserBoard>(new {ApplicationUserId = userId});
                    foreach (var linksApplicationUserBoard in linksApplicationUserBoards)
                    {
                        resultList.Add(connection.Get<Board>(linksApplicationUserBoard.BoardId));
                    }

                    return resultList;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Board GetList error");
                throw;
            }
        }
    }
}
