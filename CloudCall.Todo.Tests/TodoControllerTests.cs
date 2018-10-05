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
                new Board{ Id = 6, Title = "1list"}
            };
        }

        public List<List> GetTestLists()
        {
            return new List<List>
            {
                new List{ Id = 1, Title = "1list", BoardId = 1},
                new List{ Id = 2, Title = "1list", BoardId = 1},
                new List{ Id = 3, Title = "1list", BoardId = 1},
                new List{ Id = 4, Title = "1list", BoardId = 2},
                new List{ Id = 5, Title = "1list", BoardId = 2},
                new List{ Id = 6, Title = "1list", BoardId = 3}
            };
        }

        public List<Event> GetTestEvents()
        {
            return new List<Event>
            {
                new Event{ Id = 1, Title = "1list", ListId = 1},
                new Event{ Id = 2, Title = "1list", ListId = 1},
                new Event{ Id = 3, Title = "1list", ListId = 1},
                new Event{ Id = 4, Title = "1list", ListId = 2},
                new Event{ Id = 5, Title = "1list", ListId = 2},
                new Event{ Id = 6, Title = "1list", ListId = 3}
            };
        }

        [Fact]
        public void GetBoardReturnNotNull()
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
        public void GetBoardReturnNull()
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

        [Fact]
        public void UpdateBoardObjectChanged()
        {
            var board = new Board { Id = 1, Title = "FGHJKL" };

            var boardMock = new Mock<IStore<Board>>();
            boardMock.Setup(store => store.Update(board));
            var listMock = new Mock<IStore<List>>();
            var eventMock = new Mock<IStore<Event>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            controller.PutBoard(board);
            boardMock.Verify(store => store.Update(board));
        }

        [Fact]
        public void DeleteBoardDeleteObject()
        {
            int boardId = 1;

            var boardMock = new Mock<IStore<Board>>();
            boardMock.Setup(store => store.Delete(boardId));
            var listMock = new Mock<IStore<List>>();
            var eventMock = new Mock<IStore<Event>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            controller.DeleteBoard(boardId);
            boardMock.Verify(store => store.Delete(boardId));
        }

        [Fact]
        public void GetListReturnNotNull()
        {
            int listId = 1;

            var boardMock = new Mock<IStore<Board>>();
            var listMock = new Mock<IStore<List>>();
            listMock.Setup(store => store.Get(listId)).Returns(() => GetTestLists().FirstOrDefault(list => list.Id == listId));
            var eventMock = new Mock<IStore<Event>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            Assert.NotNull(controller.Get(listId));
        }

        [Fact]
        public void GetListReturnNull()
        {
            int listId = 0;

            var boardMock = new Mock<IStore<Board>>();
            var listMock = new Mock<IStore<List>>();
            listMock.Setup(store => store.Get(listId)).Returns(() => GetTestLists().FirstOrDefault(list => list.Id == listId));
            var eventMock = new Mock<IStore<Event>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            Assert.Null(controller.Get(listId));
        }

        [Fact]
        public void GetListRightType()
        {
            int listId = 1;

            var boardMock = new Mock<IStore<Board>>();
            var listMock = new Mock<IStore<List>>();
            listMock.Setup(store => store.Get(listId)).Returns(() => GetTestLists().FirstOrDefault(list => list.Id == listId));
            var eventMock = new Mock<IStore<Event>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            Assert.True(controller.Get(listId) is List);
        }

        [Fact]
        public void GetListRightId()
        {
            int listId = 1;

            var boardMock = new Mock<IStore<Board>>();
            var listMock = new Mock<IStore<List>>();
            listMock.Setup(store => store.Get(listId)).Returns(() => GetTestLists().FirstOrDefault(list => list.Id == listId));
            var eventMock = new Mock<IStore<Event>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            Assert.Equal(controller.Get(listId).Id, listId);
        }

        [Fact]
        public void GetListListReturnNotNull()
        {
            int boardId = 1;
            var boardMock = new Mock<IStore<Board>>();
            var listMock = new Mock<IStore<List>>();
            listMock.Setup(store => store.GetList(boardId)).Returns(() => GetTestLists().Where(list => list.BoardId == boardId));
            var eventMock = new Mock<IStore<Event>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            Assert.NotNull(controller.GetListList(boardId)); 
        }

        [Fact]
        public void GetListListNotEmpty()
        {
            int boardId = 2;
            var boardMock = new Mock<IStore<Board>>();
            var listMock = new Mock<IStore<List>>();
            listMock.Setup(store => store.GetList(boardId)).Returns(() => GetTestLists().Where(list => list.BoardId == boardId));
            var eventMock = new Mock<IStore<Event>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            Assert.NotEmpty(controller.GetListList(boardId));
        }

        [Fact]
        public void GetListListEmpty()
        {
            int boardId = 5;
            var boardMock = new Mock<IStore<Board>>();
            var listMock = new Mock<IStore<List>>();
            listMock.Setup(store => store.GetList(boardId)).Returns(() => GetTestLists().Where(list => list.BoardId == boardId));
            var eventMock = new Mock<IStore<Event>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            Assert.Empty(controller.GetListList(boardId));
        }

        [Fact]
        public void PostListObjectAdded()
        {
            int boardId = 5;
            int listId = 1;
            var listObj = new List{ Id = 0, BoardId = 0, Title = "Test"};

            var boardMock = new Mock<IStore<Board>>();
            var listMock = new Mock<IStore<List>>();
            listMock.Setup(store => store.Add(listObj, boardId)).Returns(() => listId);
            var eventMock = new Mock<IStore<Event>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            controller.Post(boardId, listObj);
            listMock.Verify(store => store.Add(listObj, boardId));
        }

        [Fact]
        public void PutListVerify()
        {
            var listObj = new List { Id = 1, BoardId = 0, Title = "Test" };

            var boardMock = new Mock<IStore<Board>>();
            var listMock = new Mock<IStore<List>>();
            listMock.Setup(store => store.Update(listObj));
            var eventMock = new Mock<IStore<Event>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            controller.Put(listObj);
            listMock.Verify(store => store.Update(listObj));
        }

        [Fact]
        public void DeleteListVerify()
        {
            int listId = 1;

            var boardMock = new Mock<IStore<Board>>();
            var listMock = new Mock<IStore<List>>();
            listMock.Setup(store => store.Delete(listId));
            var eventMock = new Mock<IStore<Event>>();
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            controller.Delete(listId);
            listMock.Verify(store => store.Delete(listId));
        }

        [Fact]
        public void GetEventListReturnNotNull()
        {
            int listId = 1;
            var boardMock = new Mock<IStore<Board>>();
            var listMock = new Mock<IStore<List>>();
            var eventMock = new Mock<IStore<Event>>();
            eventMock.Setup(store => store.GetList(listId)).Returns(() => GetTestEvents().Where(eventr => eventr.ListId == listId));
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            Assert.NotNull(controller.GetEvent(listId));
        }


        [Fact]
        public void GetEventListNotEmpty()
        {
            int listId = 1;
            var boardMock = new Mock<IStore<Board>>();
            var listMock = new Mock<IStore<List>>();
            var eventMock = new Mock<IStore<Event>>();
            eventMock.Setup(store => store.GetList(listId)).Returns(() => GetTestEvents().Where(eventr => eventr.ListId == listId));
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            Assert.NotEmpty(controller.GetEvent(listId));
        }

        [Fact]
        public void GetEventListEmpty()
        {
            int listId = 0;
            var boardMock = new Mock<IStore<Board>>();
            var listMock = new Mock<IStore<List>>();
            var eventMock = new Mock<IStore<Event>>();
            eventMock.Setup(store => store.GetList(listId)).Returns(() => GetTestEvents().Where(eventr => eventr.ListId == listId));
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            Assert.Empty(controller.GetEvent(listId));
        }

        [Fact]
        public void PostEventAddList()
        {
            int listId = 1;
            var eventObj = new Event { Id = 0, Title = "Test" };

            var boardMock = new Mock<IStore<Board>>();
            var listMock = new Mock<IStore<List>>();
            var eventMock = new Mock<IStore<Event>>();
            eventMock.Setup(store => store.Add(eventObj, listId));
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            controller.PostEvent(listId, eventObj);
            eventMock.Verify(store => store.Add(eventObj, listId));
        }

        [Fact]
        public void UpdateEventObjectChanged()
        {
            var eventObj = new Event { Id = 0, Title = "Test" };

            var boardMock = new Mock<IStore<Board>>();
            var listMock = new Mock<IStore<List>>();
            var eventMock = new Mock<IStore<Event>>();
            eventMock.Setup(store => store.Update(eventObj));
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            controller.PutEvent(eventObj);
            eventMock.Verify(store => store.Update(eventObj));
        }

        [Fact]
        public void DeleteEventDeleteObject()
        {
            int eventId = 1;

            var boardMock = new Mock<IStore<Board>>();
            var listMock = new Mock<IStore<List>>();
            var eventMock = new Mock<IStore<Event>>();
            eventMock.Setup(store => store.Delete(eventId));
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            var userMock = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            ToDoController controller = new ToDoController(listMock.Object, userMock.Object, eventMock.Object, boardMock.Object);
            controller.DeleteEvent(eventId);
            eventMock.Verify(store => store.Delete(eventId));
        }
    }
}
