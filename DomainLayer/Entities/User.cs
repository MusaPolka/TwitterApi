using DomainLayer.Enums;
using System.Text.Json.Serialization;

namespace DomainLayer.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public Accessibility AccountType { get; set; }
        public AccountStatus Status { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public string? MainPhotoUrl { get; set; }
        public ICollection<Tweet>? Tweets { get; set; }
        public ICollection<FollowerFollowing>? FollowedByUsers { get; set; }
        public ICollection<FollowerFollowing>? FollowedUsers { get; set; }
        public ICollection<SenderReciever>? SentFollowRequests { get; set; }
        public ICollection<SenderReciever>? RecievedFollowRequests { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Tweet>? Likes { get; set; }
        public ICollection<Message>? MessagesSent { get; set; }
        public ICollection<Message>? MessagesReceived { get; set; }

        public ICollection<User>? Following { get; set; }
        public ICollection<User>? Followers { get; set; }
    }
}
