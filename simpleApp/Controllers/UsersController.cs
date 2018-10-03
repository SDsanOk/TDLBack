using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CloudCall.Todo.DAL;
using CloudCall.Todo.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CloudCall.Todo.Services;
using Microsoft.AspNetCore.Cors;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace simpleApp.Controllers
{
    [EnableCors("all")]
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStore<Board> _boardStore;

        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IStore<Board> boardStore)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _boardStore = boardStore;
        }

        [HttpPost("login",Name = "login")]
        public async Task<SignInResult> Login([Required, EmailAddress] string email, [Required, DataType(DataType.Password)] string password)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(email, password, true, lockoutOnFailure: false);
                return result;
            }
            else
            {
                return null;
            }
        }

        [HttpPost("register", Name = "register")]
        public async Task<IdentityResult> Register([Required, EmailAddress] string email, [Required, DataType(DataType.Password)] string password)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {UserName = email, Email = email};
                var result = await _userManager.CreateAsync(user, password);
                return result;
            }
            else
                return null;
        }

        [HttpPost("logout", Name = "logout")]
        public async Task<IdentityResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return null;
        }

        [Authorize]
        [HttpGet("boards", Name = "getboards")]
        public IEnumerable<Board> GetBoards()
        {
            return _boardStore.GetList(int.Parse(_userManager.GetUserId(User)));
        }

    }
}
