using DomainLayer.DTOs;
using DomainLayer.Entities;
using DomainLayer.Pagination;
using DomainLayer.Pagination.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
    public interface IUserInteractionService
    {
        Task<Response<User>> Follow(RequestToFollow requestToFollow);
        Task<Response<User>> AcceptRequestToFollow(RequestToFollow requestToFollow);
        Task<Response<User>> RefuseRequestToFollow(RequestToFollow requestToFollow);
        Task<Response<User>> Unfollow(RequestToUnfollow requestToUnfollow);
        Task<Response<PagedList<string>>> GetFollowRequests(
            Guid currentUserId, FollowParams followParams);
        Task<Response<PagedList<string>>> GetFollowers(
            Guid currentUserId, FollowParams followParams);
        Task<Response<PagedList<string>>> GetFollowings(
            Guid currentUserId, FollowParams followParams);
        bool isPublicUser(User user);
        Task<bool> isAlreadyFollowed(Guid userToFollowId, Guid currentUserId);

        Task CommentTweet(CommentDto commentDto, string email);
        Task LikeTweet(Guid tweetId, string email);
        Task Retweet(Guid tweetId, string email);
    }
}
