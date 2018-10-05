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

        public BoardStore(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public int Add(Board entity, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                entity.UserId = userId;
                int boardId = connection.Insert(entity).Value;
                return boardId;
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
                var linksLists = connection.GetList<List>(new { BoardId = id });
                foreach (var linksList in linksLists)
                {
                    connection.DeleteList<Event>(new { ListId = linksList.Id });
                }
                connection.DeleteList<List>(new {BoardId = id});
                connection.DeleteList<Board>(new {Id = id});
            }
        }

        public void Update(Board entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Update<Board>(entity);
            }
        }

        public IEnumerable<Board> GetList(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.GetList<Board>(new {UserId = userId});
            }
        }
    }
}
