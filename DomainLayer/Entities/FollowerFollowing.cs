using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class FollowerFollowing : BaseEntity
    {
        public Guid SourceUserId { get; set; }
        public User SourceUser { get; set; }
        public Guid FollowedUserId { get; set; }
        public User FollowedUser { get; set; }
    }
}
