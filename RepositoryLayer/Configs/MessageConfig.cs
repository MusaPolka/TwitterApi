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
    public class MessageConfig : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatedDate).HasColumnType("datetime2");
            builder.HasOne(x => x.Sender).WithMany(x => x.MessagesSent)
                .HasForeignKey(x => x.SenderId).OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Recipient).WithMany(x => x.MessagesReceived)
                .HasForeignKey(x => x.RecipientId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
