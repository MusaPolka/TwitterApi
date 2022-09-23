using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs
{
    public class UserDto
    {
        public string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public ICollection<Tweet>? Tweets { get; set; }
    }
}
