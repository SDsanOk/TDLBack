using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using simpleApp.Models;

namespace simpleApp.Data.Stores
{
    public class TDEventStore : IStore<TDEvent>
    {
        private readonly string _connectionString;

        public TDEventStore(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Add(TDEvent entity, int listId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                int idEvent = connection.Insert(entity).Value;
                connection.Insert(new TDListTDEvent {TDEventId = idEvent, TDListId = listId});
            }
        }

        public TDEvent Get(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Get<TDEvent>(id);
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.DeleteList<TDListTDEvent>(new {TDEventId = id});
                connection.Delete<TDEvent>(id);
            }
        }

        public void Update(TDEvent entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Update(entity);
            }

        }

        public IEnumerable<TDEvent> GetListByUserId(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var resultList = new List<TDEvent>();
                var listLinksApplicationUser = connection.GetList<ApplicationUserTDList>(new { ApplicationUserId = id });
                foreach (var link in listLinksApplicationUser)
                {
                    var eventLinksList = connection.GetList<TDListTDEvent>(new {TDListId = link.TDListId});
                    foreach (var tdEvent in eventLinksList)
                    {
                        resultList.Add(connection.Get<TDEvent>(tdEvent.TDEventId));
                    }
                }

                return resultList;
            }
        }
    }
}
