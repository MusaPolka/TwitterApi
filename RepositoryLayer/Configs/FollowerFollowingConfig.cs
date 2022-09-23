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
    public class FollowerFollowingConfig : IEntityTypeConfiguration<FollowerFollowing>
    {
        public void Configure(EntityTypeBuilder<FollowerFollowing> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatedDate).HasColumnType("datetime2");
            builder.HasOne(x => x.SourceUser).WithMany(x => x.FollowedUsers).HasForeignKey(x => x.SourceUserId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.FollowedUser).WithMany(x => x.FollowedByUsers).HasForeignKey(x => x.FollowedUserId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
