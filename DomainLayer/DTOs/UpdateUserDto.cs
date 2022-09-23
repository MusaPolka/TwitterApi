    using DomainLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOs
{
    public class UpdateUserDto
    {
        
        public string? PhoneNumber { get; set; }
        public string? Bio { get; set; }
        public Accessibility AccountType { get; set; }
    }
}
