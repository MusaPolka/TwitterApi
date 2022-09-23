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
    public class TweetServiceTests
    {
        private readonly TweetService _sut;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IMapper> _mapperMock = new Mock<IMapper>();

        public TweetServiceTests()
        {
            _sut = new TweetService(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task GetAllTweets_Returns_CorrectNumbersOfTweets()
        {
            int count = 5;
            _unitOfWorkMock.Setup(x => x.Tweet.GetAllAsync()).ReturnsAsync(GetTestTweets(count));

            var actual = await _sut.GetAllTweetsAsync();

            Assert.Equal(count, actual.Count());
        }


        [Fact]
        public async Task GetAllTweets_ReturnsNull_IfTweetsIsNotExist()
        {
            _unitOfWorkMock.Setup(x => x.Tweet.GetAllAsync()).ReturnsAsync(() => null);

            var actual = await _sut.GetAllTweetsAsync();

            Assert.Null(actual);
        }

        [Theory]
        [InlineData("test@mail.com")]
        public async Task GetCurrentUserTweets_Returns_CorrectNumbersOfTweets(string email)
        {
            int expected = 5;
            _unitOfWorkMock.Setup(x => x.Tweet.GetCurrentUsersTweetsOrderByPostDateAsync(email)).ReturnsAsync(GetTestTweets(expected));

            var actual = await _sut.GetCurrentUserTweetsAsync(email);

            Assert.Equal(expected, actual.Count());
        }

        [Theory]
        [InlineData("test@mail.com")]
        public async Task GetCurrentUserTweets_Returns_EmptyList_IfUserDoesNotPostedAnything(string email)
        {
            int expected = 0;
            _unitOfWorkMock.Setup(x => x.Tweet.GetCurrentUsersTweetsOrderByPostDateAsync(email)).ReturnsAsync(GetTestTweets(expected));

            var actual = await _sut.GetCurrentUserTweetsAsync(email);

            Assert.Equal(expected, actual.Count());
        }
        [Theory]
        [InlineData("TestName")]
        public async Task GetTweetsByUserName_Returns_CorrectNumberOfTweets(string userName)
        {
            int expected = 5;

            var fakeUser = GetFakeUser(userName, DomainLayer.Enums.Accessibility.Public);

            _unitOfWorkMock.Setup(x => x.User.GetUserByNameAsync(userName)).ReturnsAsync(fakeUser);
            _unitOfWorkMock.Setup(x => x.Tweet.GetUserTweetsByUserNameOrderByPostDateAsync(userName)).ReturnsAsync(GetTestTweets(expected));


            var actual = await _sut.GetTweetsByUserNameAsync(userName);


            Assert.Equal(expected, actual.Count());
        }

        [Theory]
        [InlineData("TestName")]
        public async Task GetTweetsByUserName_ThrowsException_IfUserDoesNotExist(string userName)
        {
            _unitOfWorkMock.Setup(x => x.User.GetUserByNameAsync(userName)).ReturnsAsync(() => null);


            await Assert.ThrowsAsync<NullReferenceException>(() => _sut.GetTweetsByUserNameAsync(userName));
        }

        [Theory]
        [InlineData("TestName")]
        public async Task GetTweetsByUserName_Throws_Exception_IfUserAccountIsPrivate(string userName)
        {
            var fakeUser=GetFakeUser(userName, DomainLayer.Enums.Accessibility.Private);

            _unitOfWorkMock.Setup(x => x.User.GetUserByNameAsync(userName)).ReturnsAsync(fakeUser);

            await Assert.ThrowsAsync<Exception>(() => _sut.GetTweetsByUserNameAsync(userName));
        }


        [Theory]
        [InlineData("TestName")]
        public async Task GetTweetsByUserName_Returns_EmptyList_IfUserDoesNotPostAnything(string userName)
        {
            int expected = 0;

            var fakeUser = GetFakeUser(userName, DomainLayer.Enums.Accessibility.Public);

            _unitOfWorkMock.Setup(x => x.User.GetUserByNameAsync(userName)).ReturnsAsync(fakeUser);
            _unitOfWorkMock.Setup(x => x.Tweet.GetUserTweetsByUserNameOrderByPostDateAsync(userName)).ReturnsAsync(GetTestTweets(expected));

            var actual = await _sut.GetTweetsByUserNameAsync(userName);

            Assert.Equal(expected, actual.Count());
        }

        public IEnumerable<Tweet> GetTestTweets(int count)
        {
            var tweets = new List<Tweet>();
            Fixture fixture = new Fixture();

            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            for (int i = 0; i < count; i++)
            {

                tweets.Add(fixture.Create<Tweet>());
            }
            return tweets;
        }

        public User GetFakeUser(string userName, DomainLayer.Enums.Accessibility accountType)
        {
            Fixture fixture = new Fixture();

            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            var fakeUser = fixture
                .Build<User>()
                .With(x => x.Name, userName)
                .With(x=>x.AccountType,accountType)
                .Create();

            return fakeUser;
        }
    }
}