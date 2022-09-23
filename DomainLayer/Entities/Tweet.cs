using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Tweet : BaseEntity
    {
        //public Guid TweetId { get; set; }
        public User Owner { get; set; }
        public Guid OwnerId { get; set; } 
        public string? Content { get; set; }
        public string? AttachedFile { get; set; }
        public string? FileType { get; set; }
        //public int RetweetCount { get; set; } = 0;

        public ICollection<User>? LikedBy { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
