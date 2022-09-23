using Autofac.Extras.Moq;
using DomainLayer.Entities;
using DomainLayer.Pagination;
using DomainLayer.Pagination.Params;
using Microsoft.EntityFrameworkCore;
using Moq;
using RepositoryLayer.Repositories.Implementations;
using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Contracts;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.UnitTests
{
    public class UserServiceTests
    {
        private readonly UserService _sut;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();

        public UserServiceTests()
        {
            _sut = new UserService(_unitOfWorkMock.Object);
        }
        [Fact]
        public async Task GetAllUsersAsync_ShouldReturn_CorrectNumberOfUsers()
        {
            var expectedUsers = GetTestUsers(1, 50);

            var userParams = new UserParams
            {
                PageNumber = 1,
                PageSize = 50
            };

            _unitOfWorkMock.Setup(x => x.User.GetAllUsersAsync(userParams)).ReturnsAsync(expectedUsers);

            var actualUsers = await _sut.GetAllUsersAsync(userParams);

            Assert.Equal(expectedUsers.Count(), actualUsers.Count());
        }

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturn_Null_WhenUsersDoesntExist()
        {
            var userParams = new UserParams
            {
                PageNumber = 1,
                PageSize = 50
            };

            _unitOfWorkMock.Setup(x => x.User.GetAllUsersAsync(userParams)).ReturnsAsync(() => null);

            var actualUsers = await _sut.GetAllUsersAsync(userParams);

            Assert.Null(actualUsers);
        }

        [Theory]
        [InlineData("test", "test@test.com")]
        [InlineData("test2", "test2@test.com")]
        [InlineData("123", "123@test.com")]
        public async Task GetCurrentUserAsync_ShouldReturn_CorrectUser(string name, string email)
        {
            var user = new User()
            {
                Name = name,
                Email = email
            };

            _unitOfWorkMock.Setup(x => x.User.GetCurrentUserAsync(user.Email)).ReturnsAsync(user);

            var actualUser = await _sut.GetCurrentUserAsync(user.Email);

            Assert.Equal(user.Name, actualUser.Name);
            Assert.Equal(user.Email, actualUser.Email);
        }

        [Fact]
        public async Task GetCurrentUserAsync_ShouldReturn_Null_WhenUserDoesntExist()
        {
            _unitOfWorkMock.Setup(x => x.User.GetCurrentUserAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            var actualUser = await _sut.GetCurrentUserAsync("");

            Assert.Null(actualUser);
        }

        [Theory]
        [InlineData("test", "test@test.com")]
        [InlineData("test2", "test2@test.com")]
        [InlineData("123", "123@test.com")]
        public async Task GetUserByNameAsync_ShouldReturn_CorrectUser(string name, string email)
        {
            var user = new User()
            {
                Name = name,
                Email = email
            };

            _unitOfWorkMock.Setup(x => x.User.GetUserByNameAsync(user.Name)).ReturnsAsync(user);

            var actualUser = await _sut.GetUserByNameAsync(user.Name);

            Assert.Equal(user.Name, actualUser.Name);
            Assert.Equal(user.Email, actualUser.Email);
        }

        [Fact]
        public async Task GetUserByNameAsync_ShouldReturn_Null_WhenUserDoesntExist()
        {
            _unitOfWorkMock.Setup(x => x.User.GetUserByNameAsync(It.IsAny<string>())).ReturnsAsync(() => null);

            var actualUser = await _sut.GetUserByNameAsync("");

            Assert.Null(actualUser);
        }

        private PagedList<User> GetTestUsers(int pageNumber, int pageSize)
        {
            var users = new List<User>()
            {
                new User(){ Name = "testUser", Email = "test@test.com"},
                new User(){ Name = "testUser2", Email = "test2@test.com"},
                new User(){ Name = "testUser3", Email = "test3@test.com"}
            };

            return PagedList<User>.ToPagedList(users, pageNumber, pageSize);
        }
    }
}
