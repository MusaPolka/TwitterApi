using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class Message : BaseEntity
    {
        public Guid SenderId { get; set; }
        public string SenderUsername { get; set; }
        public User Sender { get; set; }
        public Guid RecipientId { get; set; }
        public string RecipientUsername { get; set; }
        public User Recipient { get; set; }
        public string Content { get; set; }
    }
}
