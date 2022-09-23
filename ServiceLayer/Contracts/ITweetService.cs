using DomainLayer.DTOs;
using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
    public interface ITweetService
    {
        Task CreateTweetAsync(TweetDto tweetDto, string email);
        Task<IEnumerable<TweetDto>> GetCurrentUserTweetsAsync(string email);
        Task<IEnumerable<TweetDto>> GetTweetsByUserNameAsync(string userName);
    }
}
