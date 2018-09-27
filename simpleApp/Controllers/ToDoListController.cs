using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using simpleApp.Data;
using simpleApp.Models;

namespace simpleApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoListController : ControllerBase
    {
        private readonly string _connectionString;
        private readonly IStore<TDList> _tdListStore;
        private readonly UserManager<ApplicationUser> _userManager;

        public ToDoListController(IConfiguration configuration, IStore<TDList> tdListStore, UserManager<ApplicationUser> userManager)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _tdListStore = tdListStore;
            _userManager = userManager;
        }



        // GET: api/ToDoList
        //[HttpGet]
        //public IEnumerable<string> Get(int userId)
        //{
            
        //}

        // GET: api/ToDoList/5
        [HttpGet("{id}", Name = "GetToDoList")]
        public TDList Get(int id)
        {
            return _tdListStore.Get(id);
        }

        // POST: api/ToDoList
        [HttpPost]
        [Authorize]
        public void Post()
        {
            var tdList = new TDList();
            tdList.Events.AddRange(new TDEvent[] { new TDEvent{ Title = "TEST1", Done = false}, new TDEvent { Title = "TEST2", Done = false }, new TDEvent { Title = "TEST3", Done = false } });
            tdList.Title = "TESTIK";
            
            _tdListStore.Add(tdList, int.Parse(_userManager.GetUserId(User)));
        }

        // PUT: api/ToDoList/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _tdListStore.Delete(id);
        }
    }
}
