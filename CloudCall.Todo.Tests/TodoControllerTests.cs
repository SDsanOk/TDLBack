using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CloudCall.Todo.DAL;
using CloudCall.Todo.DAL.Models;
using CloudCall.Todo.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using simpleApp.Controllers;
using Xunit;

namespace CloudCall.Todo.Tests
{
    public class TodoControllerTests
    {
        public TodoControllerTests()
        {
        }

        public List<Board> GetTestBoards()
        {
            return  new List<Board>
            {
                new Board{ Id = 1, Title = "1list"},
                new Board{ Id = 2, Title = "1list"},
                new Board{ Id = 3, Title = "1list"},
                new Board{ Id = 4, Title = "1list"},
                new Board{ Id = 5, Title = "1list"},
                new Board{ Id = 6, Title = "1list"},
            };
        }

        [Fact]
        public void GetBoardNotNull()
        {
            int boardId = 1;
            var boardMock = new Mock<IStore<Board>>();
            boardMock.Setup(store => store.Get(boardId)).Returns(() => GetTestBoards().FirstOrDefault(board => board.Id == boardId));
            var listMock = new Mock<IStore<List>>();
            var eventMock = new Mock<IStore<Event>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            Assert.NotNull(controller.GetBoard(boardId));
        }

        [Fact]
        public void GetBoardNull()
        {
            int boardId = 0;
            var boardMock = new Mock<IStore<Board>>();
            boardMock.Setup(store => store.Get(boardId)).Returns(() => GetTestBoards().FirstOrDefault(board => board.Id == boardId));
            var listMock = new Mock<IStore<List>>();
            var eventMock = new Mock<IStore<Event>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            Assert.Null(controller.GetBoard(boardId));
        }

        [Fact]
        public void GetBoardRightType()
        {
            int boardId = 1;
            var boardMock = new Mock<IStore<Board>>();
            boardMock.Setup(store => store.Get(boardId)).Returns(() => GetTestBoards().FirstOrDefault(board => board.Id == boardId));
            var listMock = new Mock<IStore<List>>();
            var eventMock = new Mock<IStore<Event>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            Assert.True(controller.GetBoard(boardId) is Board);
        }

        [Fact]
        public void GetBoardRightId()
        {
            int boardId = 1;
            var boardMock = new Mock<IStore<Board>>();
            boardMock.Setup(store => store.Get(boardId)).Returns(() => GetTestBoards().FirstOrDefault(board => board.Id == boardId));
            var listMock = new Mock<IStore<List>>();
            var eventMock = new Mock<IStore<Event>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            Assert.Equal(controller.GetBoard(boardId).Id, boardId);
        }

        [Fact]
        public void PostBoardAddBoard()
        {
            var boardMock = new Mock<IStore<Board>>();
            var listMock = new Mock<IStore<List>>();
            var eventMock = new Mock<IStore<Event>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            var board = new Board { Id = 1, Title = "FGHJKL"};

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            controller.PostBoard(board);
            boardMock.Verify(store => store.Add(board, 1));
        }
    }
}
