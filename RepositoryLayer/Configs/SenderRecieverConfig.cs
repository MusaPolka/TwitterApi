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
    public class SenderRecieverConfig : IEntityTypeConfiguration<SenderReciever>
    {
        public void Configure(EntityTypeBuilder<SenderReciever> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.CreatedDate).HasColumnType("datetime2");
            builder.HasOne(x => x.Sender).WithMany(x => x.SentFollowRequests).HasForeignKey(x => x.SenderId).OnDelete(DeleteBehavior.NoAction); ;
            builder.HasOne(x => x.Reciever).WithMany(x => x.RecievedFollowRequests).HasForeignKey(x => x.RecieverId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
