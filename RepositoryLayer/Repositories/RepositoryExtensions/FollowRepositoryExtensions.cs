using DomainLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Repositories.RepositoryExtensions
{
    public static class FollowRepositoryExtensions
    {
        public static IQueryable<SenderReciever> Search(
            this IQueryable<SenderReciever> senderRecievers, string? searchTerm)
        {
            if(string.IsNullOrWhiteSpace(searchTerm)) return senderRecievers;

            return senderRecievers
                .Where(x => x.Sender.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }
    }
}
