using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Configurations
{
    internal class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(X => X.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);

            builder.Property(X => X.Email)
                   .HasColumnType("varchar")
                   .HasMaxLength(100);

            builder.Property(X => X.Phone)
                   .HasColumnType("varchar")
                   .HasMaxLength(11);



            //Mohamed@Gmail.Com

            builder.ToTable(Tb =>
            {
                Tb.HasCheckConstraint("GymUserVaildEmailCheck", "Email like '_%@_%._%'");
                Tb.HasCheckConstraint("GymUserVaildPhoneCheck", "Phone like '01%' and Phone Not Like '%[^0-9]%' ");
            });

            builder.HasIndex(X => X.Email).IsUnique();
            builder.HasIndex(X => X.Phone).IsUnique();

            builder.OwnsOne(X => X.Address, AddressBuilder =>
            {
                AddressBuilder.Property(X => X.Street)
                              .HasColumnName("Street")
                              .HasColumnType("varchar")
                              .HasMaxLength(50);

                AddressBuilder.Property(X => X.City)
                              .HasColumnName("City")
                              .HasColumnType("varchar")
                              .HasMaxLength(50);

                AddressBuilder.Property(X => X.BuildingNumber)
                              .HasColumnName("BuildingNumber");
            });

        }
    }
}
