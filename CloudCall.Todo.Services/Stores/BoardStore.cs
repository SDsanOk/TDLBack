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


        public void Add(Board entity, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                int boardId = connection.Insert(entity).Value;
                connection.Insert(new ApplicationUserBoard {BoardId = boardId, ApplicationUserId = userId});
            }
        }

        public Board Get(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Get<Board>(id);
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Delete(id);
                connection.DeleteList<ApplicationUserBoard>(new { BoardId = id});
            }
        }

        public void Update(Board entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Update(entity);
            }
        }

        public IEnumerable<Board> GetList(int userId)
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

        public IEnumerable<Board> GetFullList(int id)
        {
            throw new NotImplementedException();
        }
    }
}
