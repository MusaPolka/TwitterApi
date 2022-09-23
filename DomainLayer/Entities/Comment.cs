using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Comment : BaseEntity
    {
        public Tweet Tweet { get; set; }
        public Guid TweetId { get; set; }
        public string Content { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public byte[]? AttachedFile { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}
