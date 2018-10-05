using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CloudCall.Todo.DAL;
using CloudCall.Todo.DAL.Models;
using CloudCall.Todo.Services.Stores;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CloudCall.Todo.Services
{
    public class ListStore : IStore<List>
    {
        private readonly string _connectionString;

        public ListStore(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public int Add(List entity, int boardId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                entity.BoardId = boardId;
                int listId = connection.Insert(entity).Value;
                return listId;
            }
        }

        public List Get(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Get<List>(id);
                result.Todo = connection.GetList<Event>(new {ListId = id}).ToList();

                return result;
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.DeleteList<Event>(new {ListId = id});
                connection.DeleteList<List>(new {Id = id});
            }
        }

        public void Update(List entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Update(entity);
            }
        }

        public IEnumerable<List> GetList(int boardId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {

                var resultList = new List<List>();
                resultList = connection.GetList<List>(new {BoardId = boardId}).ToList();
                foreach (var list in resultList)
                {
                    list.Todo = connection.GetList<Event>(new {ListId = list.Id}).ToList();
                }

                return resultList;
            }
        }
    }
}
