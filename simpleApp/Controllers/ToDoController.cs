using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CloudCall.Todo.DAL;
using CloudCall.Todo.DAL.Models;
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
        private readonly IStore<List> _listStore;
        private readonly IStore<Event> _eventStore;
        private readonly IStore<Board> _boardStore;
        private readonly UserManager<ApplicationUser> _userManager;

        public ToDoController(IConfiguration configuration, IStore<List> listStore, UserManager<ApplicationUser> userManager, IStore<Event> eventStore, IStore<Board> boardStore)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _listStore = listStore;
            _eventStore = eventStore;
            _boardStore = boardStore;
            _userManager = userManager;
        }

        //boards
        // GET: api/todo/board/5
        [HttpGet("board/{boardId}")]
        public Board GetBoard([Required] int boardId)
        {
            return _boardStore.Get(boardId);
        }

        // GET: api/todo/board/
        [HttpGet("boards/")]
        public IEnumerable<Board> GetBoardList()
        {
            return _boardStore.GetList(int.Parse(_userManager.GetUserId(User)));
        }

        // POST: api/todo/board
        [HttpPost("board/")]
        public void PostBoard([Required, FromBody] Board value)
        {
            _boardStore.Add(value, int.Parse(_userManager.GetUserId(User)));
        }

        // PUT: api/todo/board/
        [HttpPut("board/")]
        public void PutBoard([Required, FromBody] Board value)
        {
            _boardStore.Update(value);
        }

        // DELETE: api/todo/board/5
        [HttpDelete("board/{boardId}")]
        public void DeleteBoard([Required] int boardId)
        {
            _boardStore.Delete(boardId);
        }

        //lists
        // GET: api/todo/list/5
        [HttpGet("list/{listId}")]
        public List Get([Required] int listId)
        {
            return _listStore.Get(listId);
        }

        // GET: api/todo/list/5
        [HttpGet("list/{boardId}")]
        public IEnumerable<List> GetListList([Required] int boardId)
        {
            return _listStore.GetFullList(boardId);
        }

        // POST: api/todo/list
        [HttpPost("list/")]
        public void Post(int boardId,[Required, FromBody] List value)
        {     
            _listStore.Add(value, boardId);
        }

        // PUT: api/todo/list/
        [HttpPut("list/")]
        public void Put([Required, FromBody] List value)
        {
            _listStore.Update(value);
        }

        // DELETE: api/todo/list/5
        [HttpDelete("list/{listId}")]
        public void Delete([Required] int listId)
        {
            _listStore.Delete(listId);
        }

        //events
        [HttpGet("events/{listId}")]
        public IEnumerable<Event> GetEvent([Required]int listId)
        {
            return _eventStore.GetList(listId);
        }

        [HttpPost("event/{listId}")]
        public void PostEvent([Required]int listId, [Required, FromBody] Event value)
        {
            _eventStore.Add(value, listId);
        }

        [HttpDelete("event/{id}")]
        public void DeleteEvent([Required] int id)
        {
            _eventStore.Delete(id);
        }

        [HttpPut("event/")]
        public void Put([Required, FromBody] Event value)
        {
            _eventStore.Update(value);
        }
    }
}
