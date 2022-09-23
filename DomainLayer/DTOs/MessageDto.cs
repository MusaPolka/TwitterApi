using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid SenderId { get; set; }
        public string SenderUsername { get; set; }
        public Guid RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
