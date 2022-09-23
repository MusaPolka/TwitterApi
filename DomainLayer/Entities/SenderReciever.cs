using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Entities
{
    public class SenderReciever : BaseEntity
    {
        public Guid SenderId { get; set; }
        public User Sender { get; set; }
        public Guid RecieverId { get; set; }
        public User Reciever { get; set; }
    }
}
