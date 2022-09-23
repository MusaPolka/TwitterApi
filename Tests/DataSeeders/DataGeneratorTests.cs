using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.DataSeeders
{
    public static class DataGeneratorTests
    {
        public static void ReInitialize(ApplicationDbContext db)
        {
            
            db.Users.RemoveRange(db.Users);
            db.Messages.RemoveRange(db.Messages);
            Initialize(db);
        }

        public static void Initialize(ApplicationDbContext db)
        {
            db.Messages.AddRange(SeedMessages());
            //db.Users.AddRange(SeedUsers());
            db.SaveChangesAsync();
        }

        public static List<User> SeedUsers()
        {
            return new List<User>()
            {
                new User(){ Email = "test3@gmail.com", Name = "test3",
                    AccountType = DomainLayer.Enums.Accessibility.Public,
                Status = DomainLayer.Enums.AccountStatus.Active}
            };
        }

        public static List<Message> SeedMessages()
        {
            return new List<Message>()
            {
                new Message(){ Content = "test message", Sender =
                    new User{ Email = "test2@gmail.com", Name = "test2",
                        AccountType = DomainLayer.Enums.Accessibility.Public,
                        Status = DomainLayer.Enums.AccountStatus.Active },
                    SenderUsername = "test2",
                    Recipient = new User{ Email = "test@gmail.com", Name = "test",
                        AccountType = DomainLayer.Enums.Accessibility.Public,
                        Status = DomainLayer.Enums.AccountStatus.Active },
                    RecipientUsername = "test",
                }
            };
        }

    }
}
