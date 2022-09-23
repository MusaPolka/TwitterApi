using DomainLayer.Entities;
using RepositoryLayer.Repositories.RepositoryExtensions.Utilities;
using System.Linq.Dynamic.Core;

namespace RepositoryLayer.Repositories.RepositoryExtensions
{
    public static class UserRepositoryExtensions
    {
        public static IQueryable<User> Search(
            this IQueryable<User> users, string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return users;

            return users.Where(x => x.Name.ToLower().Contains(searchTerm.Trim().ToLower()));
        }

        public static IQueryable<User> Sort(
            this IQueryable<User> users, string? orderBy)
        {
            if (string.IsNullOrWhiteSpace(orderBy)) return users.OrderBy(x => x.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<User>(orderBy);

            if (string.IsNullOrWhiteSpace(orderQuery)) return users.OrderBy(x => x.Name);

            return users.OrderBy(orderQuery);
        }
    }
}
