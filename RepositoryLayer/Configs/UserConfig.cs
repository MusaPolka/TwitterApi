using DomainLayer.Entities;
using DomainLayer.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Configs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(128).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(128).IsRequired();
            builder.Property(x => x.PhoneNumber).HasMaxLength(128);
            builder.Property(x => x.Bio).HasMaxLength(256);
            builder.Property(x => x.Location).HasMaxLength(256);
            builder.Property(x => x.CreatedDate).HasColumnType("datetime2");
            builder.HasMany(x => x.Tweets).WithOne(x => x.Owner).HasForeignKey(x => x.OwnerId);
            builder.HasMany(x => x.Likes).WithMany(x => x.LikedBy).UsingEntity<Dictionary<string, object>>("LikesLikedBy", 
                j => j.HasOne<Tweet>().WithMany().OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne<User>().WithMany().OnDelete(DeleteBehavior.Restrict));
            builder.HasMany(x => x.SentFollowRequests).WithOne(x => x.Sender);
            builder.HasMany(x => x.RecievedFollowRequests).WithOne(x => x.Reciever);
            builder.HasMany(x => x.Comments).WithOne(x => x.User).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict); 

            builder.Property(x => x.AccountType).HasConversion<string>().HasDefaultValue(Accessibility.Public);
            builder.Property(x => x.Status).HasConversion<string>().HasDefaultValue(AccountStatus.Active);
        }
    }
}
