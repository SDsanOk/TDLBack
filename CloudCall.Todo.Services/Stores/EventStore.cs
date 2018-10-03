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

        public EventStore(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public int Add(Event entity, int listId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                entity.ListId = listId;
                int idEvent = connection.Insert(entity).Value;
                return connection.Insert(new ListEvent {EventId = idEvent, ListId = listId}).Value;
            }
        }

        public Event Get(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Get<Event>(id);
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.DeleteList<ListEvent>(new {EventId = id});
                connection.Delete<Event>(id);
            }
        }

        public void Update(Event entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Update(entity);
            }
        }

        public IEnumerable<Event> GetList(int listId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var resultList = new List<Event>();
                var eventLinksList = connection.GetList<ListEvent>(new {ListId = listId});
                foreach (var _event in eventLinksList)
                {
                    resultList.Add(connection.Get<Event>(_event.EventId));
                }

                return resultList;
            }
        }
    }
}
