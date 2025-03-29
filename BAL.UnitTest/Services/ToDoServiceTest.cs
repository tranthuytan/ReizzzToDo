using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BAL.UnitTest.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using ReizzzToDo.BAL.Common;
using ReizzzToDo.BAL.Extensions;
using ReizzzToDo.BAL.Filters;
using ReizzzToDo.BAL.Services.ToDoServices;
using ReizzzToDo.BAL.ViewModels.ToDoViewModels;
using ReizzzToDo.DAL.Entities;
using ReizzzToDo.DAL.Repositories.ToDoRepository;
using ReizzzToDo.DAL.Repositories.UserRepository;
using ReizzzToDo.DAL.Repositories.UserRoleRepository;

namespace BAL.UnitTest.Services
{
    public class ToDoServiceTest : HttpContextAccessorTestSetup
    {
        private readonly IToDoService _toDoService;
        private readonly IUserRepository _userRepositoryMock;
        private readonly IToDoRepository _toDoRepositoryMock;

        public ToDoServiceTest()
        {
            _userRepositoryMock = Substitute.For<IUserRepository>();
            _toDoRepositoryMock = Substitute.For<IToDoRepository>();
            _toDoService = new ToDoService(_toDoRepositoryMock, _httpContextAccessorMock, _userRepositoryMock);
        }
        [Fact]
        public async Task AddToDo_Should_ReturnSuccess()
        {
            // Arrange
            ToDoAddViewModel toDoVM = new ToDoAddViewModel
            {
                Name = "ToDo for test"
            };

            long userIdFromJwt = _httpContextAccessorMock.GetJwtSubValue();
            User user = new User
            {
                Id = 1,
                Username = "admin",
                Name = "Admin",
                PreferredTimeUnitId = 2,
            };
            _userRepositoryMock.FirstOrDefaultAsync(Arg.Is<Expression<Func<User, bool>>>(ex => ex.Compile()(user))).Returns(user);

            // Act
            var result = await _toDoService.Add(toDoVM);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().BeEquivalentTo(ToDoMessage.AddToDoSuccess);
            await _toDoRepositoryMock.Received(1).AddAsync(Arg.Is<ToDo>(td =>
                td.Name == toDoVM.Name &&
                td.TimeUnitId == user.PreferredTimeUnitId &&
                td.UserId == user.Id
            ));
            await _toDoRepositoryMock.Received(1).SaveChangesAsync();
        }
        [Fact]
        public async Task AddToDo_Should_ReturnApplication_WhenUserIdFromJwtGetNoUser()
        {
            // Arrange
            ToDoAddViewModel toDoVM = new ToDoAddViewModel
            {
                Name = "ToDo for test"
            };

            _userRepositoryMock.FirstOrDefaultAsync().ReturnsNull();

            // Act
            var result = await _toDoService.Add(toDoVM);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(ErrorMessage.UserIdNotMatchWithAnyUser);

        }
        [Fact]
        public async Task DeleteToDo_Should_ReturnSuccess()
        {
            // Arrange
            long id = 1;
            ToDo toDoToDelete = new ToDo
            {
                Id = 1,
                Name = "ToDo to delete",
                IsCompleted = false,
                UserId = 1
            };
            _toDoRepositoryMock.GetById(Arg.Is<long>(1)).Returns(toDoToDelete);

            // Act
            var result = await _toDoService.DeleteById(id);

            // Assert
            _toDoRepositoryMock.Received(1).Delete(Arg.Is<ToDo>(td =>
                td.Id == toDoToDelete.Id &&
                td.Name == toDoToDelete.Name &&
                td.IsCompleted == toDoToDelete.IsCompleted &&
                td.UserId == toDoToDelete.UserId));
            await _toDoRepositoryMock.Received(1).SaveChangesAsync();
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Be(ToDoMessage.DeleteToDoSuccess);
        }
        [Fact]
        public async Task DeleteToDo_Should_ReturnException_WhenUserIdNotMatchWithToDoUserId()
        {
            // Arrange
            long id = 1;
            ToDo toDoToDelete = new ToDo
            {
                Id = 1,
                Name = "ToDo to delete",
                IsCompleted = false,
                UserId = 2
            };
            _toDoRepositoryMock.GetById(Arg.Is(id)).Returns(toDoToDelete);

            // Act
            var result = await _toDoService.DeleteById(id);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(ErrorMessage.WrongUserAccessToDo);
        }
        [Fact]
        public async Task DeleteToDo_Should_ReturnException_WhenCantFindToDoWithThatId()
        {
            // Arrange
            long id = 1;
            ToDo toDoToDelete = new ToDo
            {
                Id = 2,
                Name = "ToDo to delete",
                IsCompleted = false,
                UserId = 2
            };
            _toDoRepositoryMock.GetById(Arg.Is(id)).ReturnsNull();

            // Act
            var result = await _toDoService.DeleteById(id);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(string.Format(ErrorMessage.NoToDoWithThatId, id));
        }
        [Fact]
        public async Task UpdateToDo_Should_ReturnSuccess()
        {
            // Arrange
            ToDo toDoToGet = new ToDo
            {
                Id = 1,
                Name = "ToDo to get",
                IsCompleted = false,
                TimeUnitId = 2,
                UserId = 1
            };

            ToDoUpdateViewModel toDoVM = new ToDoUpdateViewModel
            {
                Id = 1,
                Name = "ToDo to update",
                IsCompleted = null
            };

            _toDoRepositoryMock.GetById(Arg.Is<long>(1)).Returns(toDoToGet);

            // Act
            var result = await _toDoService.Update(toDoVM);

            // Assert
            _toDoRepositoryMock.Received(1).Update(Arg.Is<ToDo>( td=> 
                td.Id == toDoToGet.Id &&
                td.Name == toDoVM.Name &&
                td.IsCompleted == toDoToGet.IsCompleted &&
                td.TimeUnitId == toDoToGet.TimeUnitId &&
                td.UserId == toDoToGet.UserId));
            await _toDoRepositoryMock.Received(1).SaveChangesAsync();
            result.IsSuccess.Should().BeTrue();
            result.Message.Should().Contain(ToDoMessage.UpdateToDoSuccess);


        }
        [Fact]
        public async Task UpdateToDo_Should_ReturnException_WhenToDoUserIdNotMatchWithJwtSubValue()
        {
            // Arrange
            ToDo toDoToGet = new ToDo
            {
                Id = 1,
                Name = "ToDo to get",
                IsCompleted = false,
                TimeUnitId = 2,
                UserId = 2
            };

            ToDoUpdateViewModel toDoVM = new ToDoUpdateViewModel
            {
                Id = 1,
                Name = "ToDo to update",
                IsCompleted = null
            };

            _toDoRepositoryMock.GetById(Arg.Is<long>(1)).Returns(toDoToGet);

            // Act
            var result = await _toDoService.Update(toDoVM);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(ErrorMessage.WrongUserAccessToDo);
        }
        [Fact]
        public async Task UpdateToDo_Should_ReturnException_WhenUpdateBothToDoNameAndIsCompleted()
        {
            // Arrange
            ToDo toDoToGet = new ToDo
            {
                Id = 1,
                Name = "ToDo to get",
                IsCompleted = false,
                TimeUnitId = 2,
                UserId = 1
            };

            ToDoUpdateViewModel toDoVM = new ToDoUpdateViewModel
            {
                Id = 1,
                Name = "ToDo to update",
                IsCompleted = true
            };

            _toDoRepositoryMock.GetById(Arg.Is<long>(1)).Returns(toDoToGet);

            // Act
            var result = await _toDoService.Update(toDoVM);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain(ErrorMessage.ToDoCanBeUpdatedWithNameOrCompleteStateChanged);
        }
    }
}
