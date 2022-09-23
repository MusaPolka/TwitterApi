  using DomainLayer.DTOs;
using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using DomainLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Repositories.Implementations;
using ServiceLayer.Contracts;
using RepositoryLayer.Repositories.Interfaces;
using AutoMapper;

namespace ServiceLayer.Services
{
    public class TweetService : ITweetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TweetService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<Tweet>>GetAllTweetsAsync()
        {
            return await _unitOfWork.Tweet.GetAllAsync();
        }
        public async Task CreateTweetAsync(TweetDto tweetDto, string email)
        {
            Tweet tweet = new Tweet();
            tweet.Owner = await _unitOfWork.User.GetUserByEmailAsync(email);
            if (tweet.Owner == null)
            {
                await Task.FromException(new Exception("User is not exists."));
                return;
            }
            tweet.OwnerId = tweet.Owner.Id;
            tweet.Content = tweetDto.Content;
            tweet.AttachedFile = tweetDto.AttachedFile;
            tweet.CreatedDate = DateTime.Now;

            _unitOfWork.Tweet.Add(tweet);
            await _unitOfWork.SaveAsync();
        }
        public async Task<IEnumerable<TweetDto>> GetCurrentUserTweetsAsync(string email)
        {
            var tweets = await _unitOfWork.Tweet.GetCurrentUsersTweetsOrderByPostDateAsync(email);

            if (tweets == null)
                return new List<TweetDto>();
            return tweets.Select(x => _mapper.Map<TweetDto>(x));
        }

        public async Task<IEnumerable<TweetDto>> GetTweetsByUserNameAsync(string userName)
        {
            var user = await _unitOfWork.User.GetUserByNameAsync(userName);

            if (user == null)
                throw new NullReferenceException("This account doesn't exist. Try searching for another.");

            if (user.AccountType == DomainLayer.Enums.Accessibility.Private)
            {
                throw new Exception("This account is private. Subscribe to see this user's tweets.");
            }

            var tweets = await _unitOfWork.Tweet.GetUserTweetsByUserNameOrderByPostDateAsync(userName);

            if (tweets.Count() == 0)
                return new List<TweetDto>();

            return tweets.Select(x => _mapper.Map<TweetDto>(x));
        }
    }
}
