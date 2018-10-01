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
        private readonly ILogger<ListStore> _logger;

        public ListStore(IConfiguration configuration, ILogger<ListStore> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public int Add(List entity, int boardId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    entity.BoardId = boardId;
                    int listId = connection.Insert(entity).Value;
                    return connection.Insert(new BoardList {BoardId = boardId, ListId = listId}).Value;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "List Add error:");
                throw;
            }
        }

        public List Get(int id)
        {

            try
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
            catch (Exception e)
            {
                _logger.LogError(e, "TODO List Get error:");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var linkBoardLists = connection.GetList<BoardList>(new {ListId = id});
                    var linkListEvents = connection.GetList<ListEvent>(new {ListId = id});
                    connection.DeleteList<BoardList>(new { ListId = id });
                    connection.DeleteList<ListEvent>(new { ListId = id });
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
            catch (Exception e)
            {
                _logger.LogError(e, "TODO List Delete error:");
                throw;
            }
        }

        public void Update(List entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Update(entity);
            }
        }

        public IEnumerable<List> GetList(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<List> GetFullList(int boardId)
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
