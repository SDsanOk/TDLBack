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

        public void Add(TDList tdList, int? userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                int idList = connection.Insert(tdList).Value;
                connection.Insert(new ApplicationUserTDList {ApplicationUserId = userId.Value, TDListId = idList});
                foreach (var tdEvent in tdList.Events)
                {
                    int idEvent = connection.Insert(tdEvent).Value;
                    connection.Insert<TDListTDEvent>(new TDListTDEvent {TDEventId = idEvent, TDListId = idList});
                }
            }
        }

        public TDList Get(int id)
        {
            var resultTDList = new TDList();
            using (var connection = new SqlConnection(_connectionString))
            {
                var links = connection.GetList<TDListTDEvent>(new {TDListId = id});
                foreach (var a in links)
                {
                    resultTDList.Events.Add(connection.Get<TDEvent>(a.TDEventId));
                }

                return resultTDList;
            }
        }

        public void GetByUserId(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var links = connection.GetList<ApplicationUserTDList>(new { ApplicationUserId = id});
            }
        }

        public void Delete(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var links = connection.GetList<TDListTDEvent>(new { TDListId = id });
                connection.DeleteList<TDListTDEvent>(new {TDListId = id});
                connection.Delete<TDList>(id);
                foreach (var a in links)
                {
                    connection.Delete<TDEvent>(a.TDEventId);
                }
            }
        }
    }
}
