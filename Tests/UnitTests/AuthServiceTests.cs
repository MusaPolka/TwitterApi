using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Contracts;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.UnitTests
{
    public class AuthServiceTests
    {
       
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly AuthService _sut;

        public AuthServiceTests()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"Key1", "Value1"}
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();

            _sut = new AuthService(configuration, _unitOfWorkMock.Object);
        }

        /*[Fact]
        public async Task RegisterUser_ShouldRegisterCorrectUser()
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "test",
                Email = "test@test.com",
            };

            _unitOfWorkMock.Setup(x => x.User.Add(user));

            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6InRlc3QiLCJlbWFpbHMiOiJ0ZXN0QHRlc3QuY29tIiwiY291bnRyeSI6InRlc3RDb3VudHJ5IiwiaWF0IjoxNTE2MjM5MDIyfQ.dv3hLZBPX9GUc_2MddrGyGQ8sHnc-zLynH2LWMT3pCA";

            var response = await _sut.RegisterUser(token);

            Assert.Equal(response.Name, user.Name);
            Assert.Equal(response.Email, user.Email);
        }*/

        [Fact]
        public async Task RegisterUser_ShouldReturnNull_WhenUserIsAlreadyExists()
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "test",
                Email = "test@test.com",
            };

            _unitOfWorkMock.Setup(x => x.User.Add(user));

            _unitOfWorkMock.Setup(x => x.User.UserExistsAsync(user.Email)).ReturnsAsync(true);

            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6InRlc3QiLCJlbWFpbHMiOiJ0ZXN0QHRlc3QuY29tIiwiY291bnRyeSI6InRlc3RDb3VudHJ5IiwiaWF0IjoxNTE2MjM5MDIyfQ.dv3hLZBPX9GUc_2MddrGyGQ8sHnc-zLynH2LWMT3pCA";

            var response = await _sut.RegisterUser(token);

            Assert.Null(response);
        }

        [Fact]
        public void GetClaimsFromToken_ShouldReturnCorrectClaims()
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = "test",
                Email = "test@test.com",
                Location = "testCountry"
            };

            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6InRlc3QiLCJlbWFpbHMiOiJ0ZXN0QHRlc3QuY29tIiwiY291bnRyeSI6InRlc3RDb3VudHJ5IiwiaWF0IjoxNTE2MjM5MDIyfQ.dv3hLZBPX9GUc_2MddrGyGQ8sHnc-zLynH2LWMT3pCA";

            var response = _sut.GetClaimsFromToken(token);

            var actualName = response
                .FirstOrDefault(x => x.Type == "name").Value;

            var actualEmail = response
                .FirstOrDefault(x => x.Type == "emails").Value;

            var actualCountry = response
                .FirstOrDefault(x => x.Type == "country").Value;

            Assert.NotNull(response);
            Assert.Equal(user.Email, actualEmail);
            Assert.Equal(user.Name, actualName);
            Assert.Equal(user.Location, actualCountry);
        }

        [Fact]
        public async Task UserExists_ShouldReturnTrue_WhenUserExists()
        {
            var email = "test@test.com";

            _unitOfWorkMock.Setup(x =>
                x.User.UserExistsAsync(email)).ReturnsAsync(true);

            var response = await _sut.UserExists(email);

            Assert.True(response);
        }

        [Fact]
        public async Task UserExists_ShouldReturnFalse_WhenUserDoesntExist()
        {
            var email = "test@test.com";

            _unitOfWorkMock.Setup(x =>
                x.User.UserExistsAsync(email)).ReturnsAsync(false);

            var response = await _sut.UserExists(email);

            Assert.False(response);
        }
    }
}
