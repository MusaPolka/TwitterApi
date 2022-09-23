using DomainLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Configs
{
    public class TweetConfig : IEntityTypeConfiguration<Tweet>
    {
        public void Configure(EntityTypeBuilder<Tweet> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatedDate).HasColumnType("datetime2");
            builder.Property(x => x.Content).HasMaxLength(140).IsRequired();
            builder.Property(x => x.AttachedFile);
            builder.Property(x => x.FileType);
            builder.HasOne(x => x.Owner).WithMany(x => x.Tweets);

            builder.HasMany(x => x.LikedBy).WithMany(x => x.Likes);

            //builder.HasMany(x => x.LikedBy).WithMany(x => x.Likes).HasForeignKey(x => x.TweetId);
            builder.HasMany(x => x.Comments).WithOne(x => x.Tweet).HasForeignKey(x => x.TweetId);
        }
    }
}
