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
    public class ConfigSpeach : IEntityTypeConfiguration<Speach>
    {
        public void Configure(EntityTypeBuilder<Speach> builder)
        {
            builder.HasKey(t => t.Id);

        }
    }
}
