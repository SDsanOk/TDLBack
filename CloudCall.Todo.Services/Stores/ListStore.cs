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
                return connection.Insert(new BoardList {BoardId = boardId, ListId = listId}).Value;
            }
        }

        public List Get(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var result = connection.Get<List>(id);
                var linksListEvents = connection.GetList<ListEvent>(new {ListId = id});
                foreach (var listEvent in linksListEvents)
                {
                    result.Todo.Add(connection.Get<Event>(listEvent.EventId));
                }

                return result;
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var linkBoardLists = connection.GetList<BoardList>(new {ListId = id});
                var linkListEvents = connection.GetList<ListEvent>(new {ListId = id});
                connection.DeleteList<BoardList>(new {ListId = id});
                connection.DeleteList<ListEvent>(new {ListId = id});
                foreach (var boardList in linkBoardLists)
                {
                    connection.Delete<List>(boardList.ListId);
                }

                foreach (var listEvent in linkListEvents)
                {
                    connection.Delete<Event>(listEvent.EventId);
                }
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
                var linksBoardList = connection.GetList<BoardList>(new {BoardId = boardId});
                foreach (var boardList in linksBoardList)
                {
                    var tempList = connection.Get<List>(boardList.ListId);
                    var linksListEvents = connection.GetList<ListEvent>(new {ListId = boardList.ListId});
                    foreach (var listEvent in linksListEvents)
                    {
                        tempList.Todo.Add(connection.Get<Event>(listEvent.EventId));
                    }

                    resultList.Add(tempList);
                }

                return resultList;
            }
        }
    }
}
