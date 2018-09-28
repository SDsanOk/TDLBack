using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using simpleApp.Models;

namespace simpleApp.Data
{
    public class TDListStore : IStore<TDList>
    {
        private readonly string _connectionString;

        public TDListStore(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Add(TDList tdList, int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                int idList = connection.Insert(tdList).Value;
                connection.Insert(new ApplicationUserTDList {ApplicationUserId = userId, TDListId = idList});
                foreach (var tdEvent in tdList.Events)
                {
                    int idEvent = connection.Insert(tdEvent).Value;
                    connection.Insert<TDListTDEvent>(new TDListTDEvent {TDEventId = idEvent, TDListId = idList});
                }
            }
        }

        public TDList Get(int id)
        {
            
            using (var connection = new SqlConnection(_connectionString))
            {
                var resultTDList = connection.Get<TDList>(id);
                var links = connection.GetList<TDListTDEvent>(new {TDListId = id});
                foreach (var a in links)
                {
                    resultTDList.Events.Add(connection.Get<TDEvent>(a.TDEventId));
                }

                return resultTDList;
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var links = connection.GetList<TDListTDEvent>(new { TDListId = id });
                connection.DeleteList<TDListTDEvent>(new {TDListId = id});
                connection.DeleteList<ApplicationUserTDList>(new { TDListId = id });
                connection.Delete<TDList>(id);
                foreach (var a in links)
                {
                    connection.Delete<TDEvent>(a.TDEventId);
                }
            }
        }

        public void Update(TDList entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                //todo: Delete->Insert or Update
                connection.Update(entity);
                foreach (var entityEvent in entity.Events)
                {
                    if (connection.Get<TDEvent>(entityEvent.Id) == null)
                    {
                        int idEvent = connection.Insert(entityEvent).Value;
                        connection.Insert(new TDListTDEvent {TDEventId = idEvent, TDListId = entity.Id});
                    }
                    else
                    {
                        connection.Update(entityEvent);
                        connection.Update(new TDListTDEvent { TDEventId = entityEvent.Id, TDListId = entity.Id });
                    }
                    
                }
            }
        }

        public IEnumerable<TDList> GetListByUserId(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var resultList = new List<TDList>();
                var listLinksApplicationUser = connection.GetList<ApplicationUserTDList >(new {ApplicationUserId = id});
                foreach (var link in listLinksApplicationUser)
                {
                    var tdList = connection.Get<TDList>(link.TDListId);
                    var eventLinksList = connection.GetList<TDListTDEvent>(new { TDListId = link.TDListId });
                    foreach (var tdEvent in eventLinksList)
                    {
                        tdList.Events.Add(connection.Get<TDEvent>(tdEvent.TDEventId));
                    }
                    resultList.Add(tdList);
                }

                return resultList;
            }
            
        }
    }
}
