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
    public class ModelConfiguration : IEntityTypeConfiguration<DSS2022.Model.Model>
    {
        void IEntityTypeConfiguration<DSS2022.Model.Model>.Configure(EntityTypeBuilder<DSS2022.Model.Model> builder)
        {
            builder
                .Property(a => a.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(a => a.Description)
                .HasMaxLength(200)
                .IsRequired();

            builder
                .HasOne<Collection>(x => x.Collection)
                .WithMany(x => x.Models)
                .HasForeignKey(x => x.CollectionId);


        }
    }
}
