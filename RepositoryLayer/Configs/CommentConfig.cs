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
    public class CommentConfig : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatedDate).HasColumnType("datetime2");
            builder.Property(x => x.Content).HasMaxLength(140).IsRequired();
            builder.Property(x => x.AttachedFile);
            builder.HasOne(x => x.Tweet).WithMany(x => x.Comments);
            builder.HasOne(x => x.User).WithMany(x => x.Comments);
            builder.HasMany(x => x.Comments);
        }
    }
}
