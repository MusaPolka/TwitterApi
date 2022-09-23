using DomainLayer.Entities;
using DomainLayer.Enums;
using DomainLayer.Pagination;
using DomainLayer.Pagination.Params;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Implementations;
using RepositoryLayer.Repositories.Interfaces;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedList<User>> GetAllUsersAsync(UserParams userParams)
        {
            return await _unitOfWork.User.GetAllUsersAsync(userParams);
        }

        public async Task<User> GetCurrentUserAsync(string name)
        {
            return await _unitOfWork.User.GetCurrentUserAsync(name);
        }

        public async Task<User> GetUserByNameAsync(string userName)
                {
            return await _unitOfWork.User.GetUserByNameAsync(userName);
            }

        public async Task Save()
        {
            await _unitOfWork.SaveAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            var user = await _unitOfWork.User.GetByIdAsync(userId);
            if (user == null)
                throw new Exception("This account doesn't exist.");
            return user;
        }
        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            var user = await _unitOfWork.User.GetUserByNameAsync(userName);
            if (user == null)
                throw new Exception("This account doesn't exist.");
            return user;
        }

        public async Task<IEnumerable<Tweet>> GetFollowingUserTweetsAsync(string userName)
        {
            var followingUsers = await _unitOfWork.User.GetFollowingUsersAsync(userName);
            var followingUsersTweets = new List<Tweet>();
            if (followingUsers == null)
                return new List<Tweet>();
            else
            {
                foreach (var user in followingUsers)
                {
                    followingUsersTweets.AddRange(await _unitOfWork.Tweet.GetUserTweetsByUserNameOrderByPostDateAsync(user.Name));
                }
                followingUsers.OrderBy(x => x.CreatedDate);
            }
            return followingUsersTweets;
        }
    }
}
