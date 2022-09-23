using AutoFixture;
using DomainLayer.DTOs;
using Moq;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitterAPI.Controllers;
using Xunit;

namespace Tests.ControllerTests
{
    public class TweetControllerTests
    {
        private readonly Mock<ITweetService> _tweetService = new Mock<ITweetService>();

        private readonly TweetController tweetController;
        public TweetControllerTests()
        {
            tweetController = new TweetController(_tweetService.Object);
        }
    }
}
