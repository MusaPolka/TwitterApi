using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs
{
    public class CreateMessageDto
    {
        public string ResipientName { get; set; }
        public string Message { get; set; }
    }
}
