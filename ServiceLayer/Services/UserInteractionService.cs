using AutoMapper;
using DomainLayer.DTOs;
using DomainLayer.Entities;
using DomainLayer.Enums;
using DomainLayer.Pagination;
using DomainLayer.Pagination.Params;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Context;
using RepositoryLayer.Repositories.Implementations;
using RepositoryLayer.Repositories.Interfaces;
using RepositoryLayer.Repositories.RepositoryExtensions;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Services
{
    public class UserInteractionService : IUserInteractionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserInteractionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response<User>> Follow(RequestToFollow requestToFollow)
        {
            var userToFollow = requestToFollow.FollowedUser;
            var currentUser = requestToFollow.SourceUser;

            if (await isAlreadyFollowed(userToFollow.Id, currentUser.Id))
            {
                return new Response<User>(false,
                $"You have already followed {userToFollow.Name}",
                currentUser);
            }

            if (isPublicUser(userToFollow))
            {
                var followerFollowing = _mapper.Map<FollowerFollowing>(requestToFollow);

                _unitOfWork.FollowerFollowing.Add(followerFollowing);

                await _unitOfWork.SaveAsync();

                return new Response<User>(true,
                    $"You have successfully followed {userToFollow.Name}",
                    currentUser);
            }

            if (await isAlreadySendRequest(userToFollow.Id, currentUser.Id))
            {
                return new Response<User>(false,
                    $"You have already sent request to follow to {userToFollow.Name}",
                    currentUser);
            }

            var senderReciever = _mapper.Map<SenderReciever>(requestToFollow);

            _unitOfWork.SenderReciever.Add(senderReciever);

            await _unitOfWork.SaveAsync();

            return new Response<User>(true,
                    $"You have successfully send follow request to {userToFollow.Name}",
                    currentUser);
        }

        public async Task<Response<User>> AcceptRequestToFollow(RequestToFollow requestToFollow)
        {
            var users = await _unitOfWork.User.GetAllAsIQueryable().ToListAsync();

            var userToAccept = requestToFollow.SourceUser;
            var currentUser = requestToFollow.FollowedUser;

            var followerFollowing = _mapper.Map<FollowerFollowing>(requestToFollow);

            var request = await GetSpecificRequest(userToAccept.Id, currentUser.Id);

            if (request == null)
            {
                return new Response<User>(false,
                "Current user does not have any follow requests to accept them",
                currentUser);
            }

            _unitOfWork.FollowerFollowing.Add(followerFollowing);

            _unitOfWork.SenderReciever.Delete(request);

            await _unitOfWork.SaveAsync();

            return new Response<User>(true,
                    $"You have successfully accepted follow request from {userToAccept.Name}",
                    currentUser);
        }

        public async Task<Response<User>> RefuseRequestToFollow(RequestToFollow requestToFollow)
        {
            var users = await _unitOfWork.User.GetAllAsIQueryable().ToListAsync();

            var userToRefuse = requestToFollow.SourceUser;
            var currentUser = requestToFollow.FollowedUser;

            var request = await GetSpecificRequest(userToRefuse.Id, currentUser.Id);

            if (request == null)
            {
                return new Response<User>(false,
                "Current user does not have any follow requests to refuse them",
                currentUser);
            }

            _unitOfWork.SenderReciever.Delete(request);

            await _unitOfWork.SaveAsync();

            return new Response<User>(true,
                    $"You have successfully refused follow request from {userToRefuse.Name}",
                    currentUser);
        }

        public async Task<Response<PagedList<string>>> GetFollowRequests(
            Guid currentUserId, FollowParams followParams)
        {
            var users = await _unitOfWork.User.GetAllAsIQueryable().ToListAsync();

            var requests = await _unitOfWork.SenderReciever
                .GetByCondition(x => x.RecieverId == currentUserId)
                .Search(followParams.SearchTerm)
                .ToListAsync();

            if (requests == null)
            {
                return new Response<PagedList<string>>(false,
                "Current user does not have any follow requests",
                null);
            }

            var requestsName = new List<string>();

            foreach (var item in requests)
            {
                requestsName.Add(item.Sender.Name);
            }

            var pagedRequests = PagedList<string>
                .ToPagedList(requestsName, followParams.PageNumber, followParams.PageSize);

            return new Response<PagedList<string>>(true,
                "All the Follow requests",
                pagedRequests);
        }

        public async Task<Response<User>> Unfollow(RequestToUnfollow requestToUnfollow)
        {
            var userToUnfollow = requestToUnfollow.UnfollowedUser;
            var currentUser = requestToUnfollow.SourceUser;

            if (!await isAlreadyFollowed(userToUnfollow.Id, currentUser.Id))
            {
                return new Response<User>(false,
                $"You have not followed {userToUnfollow.Name} yet",
                currentUser);
            }

            var followerFollowing = await _unitOfWork.FollowerFollowing
                .GetByCondition(x => x.SourceUserId == currentUser.Id && 
                x.FollowedUserId == userToUnfollow.Id).SingleOrDefaultAsync();

            _unitOfWork.FollowerFollowing.Delete(followerFollowing);

            await _unitOfWork.SaveAsync();

            return new Response<User>(true,
                $"You have successfully unfollowed {userToUnfollow.Name}",
                currentUser);
        }

        public async Task<Response<PagedList<string>>> GetFollowings(
            Guid currentUserId, FollowParams followParams)
        {
            var users = await _unitOfWork.User.GetAllAsIQueryable().ToListAsync();

            var requests = await _unitOfWork.FollowerFollowing
                .GetByCondition(x => x.SourceUserId == currentUserId)
                .ToListAsync();

            if (requests == null)
            {
                return new Response<PagedList<string>>(false,
                $"Current user does not have any followings",
                null);
            }

            var requestsName = new List<string>();

            foreach (var item in requests)
            {
                requestsName.Add(item.FollowedUser.Name);
            }

            var pagedRequests = PagedList<string>
                .ToPagedList(requestsName, followParams.PageNumber, followParams.PageSize);

            return new Response<PagedList<string>>(true,
                "All the Followings",
                pagedRequests);
        }

        public async Task<Response<PagedList<string>>> GetFollowers(
            Guid currentUserId, FollowParams followParams)
        {
            var users = await _unitOfWork.User.GetAllAsIQueryable().ToListAsync();

            var requests = await _unitOfWork.FollowerFollowing
                .GetByCondition(x => x.FollowedUserId == currentUserId)
                .ToListAsync();

            if (requests == null)
            {
                return new Response<PagedList<string>>(false,
                $"Current user does not have any followers",
                null);
            }

            var requestsName = new List<string>();

            foreach (var item in requests)
            {
                requestsName.Add(item.SourceUser.Name);
            }

            var pagedRequests = PagedList<string>
                .ToPagedList(requestsName, followParams.PageNumber, followParams.PageSize);

            return new Response<PagedList<string>>(true,
                "All the Followers",
                pagedRequests);
        }

        public bool isPublicUser(User user)
        {
            return user.AccountType == Accessibility.Public;
        }

        public async Task<bool> isAlreadyFollowed(Guid userToFollowId, Guid currentUserId)
        {
            return await _unitOfWork.FollowerFollowing
                .GetByCondition(x => x.SourceUserId == currentUserId && x.FollowedUserId == userToFollowId)
                .AnyAsync();
        }

        public async Task<bool> isAlreadySendRequest(Guid userToFollowId, Guid currentUserId)
        {
            return await _unitOfWork.SenderReciever
                .GetByCondition(x => x.SenderId == currentUserId && x.RecieverId == userToFollowId)
                .AnyAsync();
        }

        public async Task<SenderReciever> GetSpecificRequest(Guid userToFollowId, Guid currentUserId)
        {
            return await _unitOfWork.SenderReciever
                .GetByCondition(x => x.SenderId == userToFollowId && x.RecieverId == currentUserId)
                .SingleOrDefaultAsync();
        }


        public async Task CommentTweet(CommentDto commentDto, string email)
        {
            var user = await _unitOfWork.User.GetUserByEmailAsync(email);

            Comment comment = new Comment();

            comment.Tweet = commentDto.Tweet;
            comment.AttachedFile = commentDto.AttachedFile;
            comment.Content = commentDto.Content;
            comment.CreatedDate = DateTime.Now;
            comment.TweetId = commentDto.Tweet.Id;
            comment.User = user;
            comment.Id = user.Id;

            _unitOfWork.Comment.Add(comment);

            await _unitOfWork.SaveAsync();
        }
        public async Task LikeTweet(Guid tweetId, string email)
        {
            var tweet = await _unitOfWork.Tweet.GetByIdAsync(tweetId);

            if (tweet == null)
                throw new NullReferenceException("This tweet is not exist.");

            var user = await _unitOfWork.User.GetUserByEmailAsync(email);

            if (tweet.OwnerId == user.Id)
                throw new Exception("You can not click Like button to your own tweet!");

            if (user.Likes == null)
                user.Likes = new List<Tweet>();

            user.Likes.Add(tweet);
            tweet.LikedBy.Add(user);

            await _unitOfWork.SaveAsync();
        }
        public async Task Retweet(Guid tweetId, string email)
        {
            var tweet = await _unitOfWork.Tweet.GetByIdAsync(tweetId);

            if (tweet == null)
                throw new Exception("This tweet is not exist.");

            var user = await _unitOfWork.User.GetUserByEmailAsync(email);

            user.Tweets.Add(tweet);

            //tweet.RetweetCount++;

            await _unitOfWork.SaveAsync();
        }

    }
}
