using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Configs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FollowerFollowing> FollowersFollowings { get; set; }
        public DbSet<SenderReciever> SendersRecievers { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserConfig());
            builder.ApplyConfiguration(new TweetConfig());
            builder.ApplyConfiguration(new CommentConfig());
            builder.ApplyConfiguration(new FollowerFollowingConfig());
            builder.ApplyConfiguration(new SenderRecieverConfig());
            builder.ApplyConfiguration(new MessageConfig());
        }
    }
}
