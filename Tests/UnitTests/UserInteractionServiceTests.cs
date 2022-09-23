using AutoFixture;
using AutoMapper;
using DomainLayer.Entities;
using Moq;
using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.UnitTests
{
    public class UserInteractionServiceTests
    {
        private readonly UserInteractionService _sut;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();

        public UserInteractionServiceTests()
        {
            _sut = new UserInteractionService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task LikeTweet_ThrowsNullReferenceException_IfTweetIsNotExist()
        {
            var tweetId = new Guid();
            _unitOfWorkMock.Setup(x => x.Tweet.GetByIdAsync(tweetId)).ReturnsAsync(() => null);

            await Assert.ThrowsAsync<NullReferenceException>(() => _sut.LikeTweet(tweetId, "test@mail.com"));
        }

        [Fact]
        public async Task LikeTweet_ThrowsException_IfLikeOwnTweet()
        {
            var tweetId = new Guid();
            var userId = new Guid();
            string email = "test@mail.com";

            _unitOfWorkMock.Setup(x => x.Tweet.GetByIdAsync(tweetId)).ReturnsAsync(GetFakeTweet(userId));
            _unitOfWorkMock.Setup(x => x.User.GetUserByEmailAsync(email)).ReturnsAsync(GetFakeUser(userId));

            var exception =await Assert.ThrowsAsync<Exception>(() => _sut.LikeTweet(tweetId, "test@mail.com"));
            Assert.Equal("You can not click Like button to your own tweet!", exception.Message);
        }

        public Tweet GetFakeTweet(Guid userId)
        {
            Fixture fixture = new Fixture();

            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var fakeTweet = fixture
                .Build<Tweet>()
                .With(x => x.OwnerId, userId)
                .Create();

            return fakeTweet;
        }
        public User GetFakeUser(Guid userId)
        {
            Fixture fixture = new Fixture();

            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var fakeUser = fixture
                .Build<User>()
                .With(x => x.Id, userId)
                .Create();

            return fakeUser;
        }
    }
}
