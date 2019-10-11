using InfrustructureData.DataModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrustructureData.Configuration
{
    public class ConfigMessage : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {

            builder.HasKey(t =>t.Id);

            builder
                .HasOne(sc => sc.Speach)
                .WithMany(s => s.Messages)
                .HasForeignKey(sc => sc.SpeachId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
