using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs
{
    public class RequestToUnfollow
    {
        public Guid UnfollowedUserId { get; set; }
        public User UnfollowedUser { get; set; }
        public Guid SourceUserId { get; set; }
        public User SourceUser { get; set; }
    }
}
