using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs
{
    public class CommentDto
    {
        public Tweet Tweet { get; set; }
        public string Content { get; set; }
        public User User { get; set; }
        public Guid UserId { get; set; }
        public byte[]? AttachedFile { get; set; }
    }
}
