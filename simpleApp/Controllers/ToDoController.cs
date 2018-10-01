using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CloudCall.Todo.DAL;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using CloudCall.Todo.Services;

namespace simpleApp.Controllers
{
    [Route("api/todo")]
    [Authorize]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly IStore<TDList> _tdListStore;
        private readonly IStore<TDEvent> _tdEventStore;
        private readonly UserManager<ApplicationUser> _userManager;

        public ToDoController(IConfiguration configuration, IStore<TDList> tdListStore, UserManager<ApplicationUser> userManager, IStore<TDEvent> tdEventStore)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _tdListStore = tdListStore;
            _tdEventStore = tdEventStore;
            _userManager = userManager;
        }

        // GET: api/todo/list/5
        [HttpGet("list/{id}", Name = "GetToDoList")]
        public TDList Get(int id)
        {
            return _tdListStore.Get(id);
        }

        // POST: api/todo/list
        [HttpPost("list/")]
        public void Post([FromBody] TDList value)
        {     
            _tdListStore.Add(value, int.Parse(_userManager.GetUserId(User)));
        }

        // PUT: api/todo/list/
        [HttpPut("list/")]
        public void Put([FromBody] TDList value)
        {
            _tdListStore.Update(value);
        }

        // DELETE: api/todo/list/5
        [HttpDelete("list/{id}")]
        public void Delete(int id)
        {
            _tdListStore.Delete(id);
        }

        //events

        [HttpPost("event/{id}")]
        public void PostEvent(int id, [FromBody] TDEvent value)
        {
            _tdEventStore.Add(value, id);
        }

        [HttpDelete("event/{id}")]
        public void DeleteEvent(int id)
        {
            _tdEventStore.Delete(id);
        }

        [HttpPut("event/")]
        public void Put([FromBody] TDEvent value)
        {
            _tdEventStore.Update(value);
        }
    }
}
