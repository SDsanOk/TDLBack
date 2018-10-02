using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CloudCall.Todo.DAL;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


namespace CloudCall.Todo.Services.Stores
{
    public class EventStore : IStore<Event>
    {
        private readonly string _connectionString;
        private readonly ILogger<EventStore> _logger;

        public EventStore(IConfiguration configuration, ILogger<EventStore> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }

        public int Add(Event entity, int listId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    entity.ListId = listId;
                    int idEvent = connection.Insert(entity).Value;
                    return connection.Insert(new ListEvent {EventId = idEvent, ListId = listId}).Value;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Event Add error");
                throw;
            }
        }

        public Event Get(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    return connection.Get<Event>(id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Event Get error");
                throw;
            }
        }

        public void Delete(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.DeleteList<ListEvent>(new {EventId = id});
                    connection.Delete<Event>(id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Event Delete error");
                throw;
            }
        }

        public void Update(Event entity)
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
                _logger.LogError(e, "Event Update error");
                throw;
            }

        }

        public IEnumerable<Event> GetList(int listId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var resultList = new List<Event>();
                    var eventLinksList = connection.GetList<ListEvent>(new { ListId = listId });
                    foreach (var _event in eventLinksList)
                    {
                        resultList.Add(connection.Get<Event>(_event.EventId));
                    }

                    return resultList;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Event GetList error");
                throw;
            }
        }
    }
}
