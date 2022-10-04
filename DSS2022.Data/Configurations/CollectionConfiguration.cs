using DSS2022.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSS2022.Data.Configurations
{
    public class CollectionConfiguration : IEntityTypeConfiguration<Collection>
    {
        void IEntityTypeConfiguration<Collection>.Configure(EntityTypeBuilder<Collection> builder)
        {
            builder
                .Property(a => a.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.Description)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(a => a.FechaLanzamientoEstimada)
                .IsRequired();

        }
    }
}
