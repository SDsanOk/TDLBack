using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using simpleApp.Data;
using simpleApp.Models;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace simpleApp.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRoleStore<ApplicationRole> _roleStore;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UsersController(IRoleStore<ApplicationRole> roleStore, IUserStore<ApplicationUser> userStore, UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
        {
            _roleStore = roleStore;
            _userStore = userStore;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        //sdelat` OAuth
        [HttpGet("login",Name = "login")]
        public async Task<SignInResult> Login(string email, string  password)
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
        public async Task<IdentityResult> Register(string email, string password)
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

        [HttpGet("logout", Name = "logout")]
        public async Task<IdentityResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return null;
        }

        // GET: api/Users/5
        [HttpGet("{id}", Name = "GetUser")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Users
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

    }
}
