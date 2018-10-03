using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CloudCall.Todo.DAL;
using CloudCall.Todo.DAL.Models;
using CloudCall.Todo.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using simpleApp.Controllers;
using Xunit;

namespace CloudCall.Todo.Tests
{
    public class UserControllerTests
    {
        private Mock<IStore<Board>> a;
        private Mock<UserManager<ApplicationUser>> b;
        private Mock<SignInManager<ApplicationUser>> c;

        public UserControllerTests()
        {
        }

        [Fact]
        public void RegisterResultNotNull()
        {
            UsersController usersController = new UsersController(b.Object, c.Object, a.Object);
            IdentityResult result = usersController.Register("test@mail.com", "somePassword_123").Result;
            Assert.NotNull(result);
        }

        [Fact]
        public void RegisterResultFalse()
        {
            UsersController usersController = new UsersController(b.Object, c.Object, a.Object);
            IdentityResult result = usersController.Register("test@mail.com", "somePassword_123").Result;
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void LoginResultNotNull()
        {
            UsersController usersController = new UsersController(b.Object, c.Object, a.Object);
            SignInResult result = usersController.Login("test@mail.com", "hV7#`zavR.5F\"QyU").Result;
            Assert.NotNull(result);
        }

        [Fact]
        public void LoginResultFalse()
        {
            UsersController usersController = new UsersController(b.Object, c.Object, a.Object);
            SignInResult result = usersController.Login("test65@mail.com", "hV7#`zavR.5F\"QyU").Result;
            Assert.False(result.Succeeded);
        }

        [Fact]
        public void LoginResultTrue()
        {
            UsersController usersController = new UsersController(b.Object, c.Object, a.Object);
            SignInResult result = usersController.Login("test@mail.com", "hV7#`zavR.5F\"QyU").Result;
            Assert.True(result.Succeeded);
        }

        [Fact]
        public void LogoutResultNull()
        {
            UsersController usersController = new UsersController(b.Object, c.Object, a.Object);
            IdentityResult result = usersController.LogOut().Result;
            Assert.Null(result);
        }

        [Fact]
        public void LogoutResult()
        {
            UsersController usersController = new UsersController(b.Object, c.Object, a.Object);
            LoginResultTrue();
            Console.WriteLine(b.Object.GetUserId(usersController.User));
        }
    }
}
