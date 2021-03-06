﻿using System;
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
using CloudCall.Todo.ViewModels;
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
        public async Task<dynamic> Login([FromBody] LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid)
            {
                string cookieValue = string.Empty;
                var result = await _signInManager.PasswordSignInAsync(loginViewModel.Email, loginViewModel.Password, true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var cookie = Response.Headers.FirstOrDefault(_ => _.Key == "Set-Cookie");
                    var header = cookie.Value[0];
                    var p1 = header.IndexOf('=');
                    var p2 = header.IndexOf(';');
                    cookieValue = header.Substring(p1 + 1, p2 - p1 - 1);
                    Response.Headers.Remove(cookie);
                }
                return new {result, cookieValue};
            }
            else
            {
                return null;
            }
        }

        [HttpPost("register", Name = "register")]
        public async Task<IdentityResult> Register([FromBody] RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {UserName = registerViewModel.Email, Email = registerViewModel.Email};
                var result = await _userManager.CreateAsync(user, registerViewModel.Password);
                return result;
            }
            else
                return null;
        }

        [HttpPost("logout", Name = "logout")]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

        [Authorize]
        [HttpGet("boards", Name = "getboards")]
        public IEnumerable<Board> GetBoards()
        {
            return _boardStore.GetList(int.Parse(_userManager.GetUserId(User)));
        }

    }
}
