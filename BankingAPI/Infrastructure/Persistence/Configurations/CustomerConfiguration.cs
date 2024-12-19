using BankingAPI.Domain.Entities;
using BankingAPI.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");

        // Customer fields
        builder.Property(x => x.FullName)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("full_name")
            .HasColumnType("varchar(100)");

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(255)
            .HasColumnName("email")
            .HasColumnType("varchar(255)");

        builder.HasIndex(x => x.Email).IsUnique();

        builder.Property(x => x.Password)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnName("password")
            .HasColumnType("varchar(100)");

        builder.Property(x => x.NationalNumber)
            .IsRequired()
            .HasMaxLength(11)
            .IsFixedLength()
            .HasColumnName("national_number");

        builder.HasIndex(x => x.NationalNumber).IsUnique();

        builder.Property(x => x.PlaceOfBirth)
            .IsRequired()
            .HasColumnType("varchar(100)")
            .HasColumnName("place_of_birth");

        builder.Property(x => x.DateOfBirth)
            .IsRequired()
            .HasColumnName("date_of_birth");

        builder.Property(x => x.RiskLimit)
            .IsRequired()
            .HasDefaultValue(10000.00)
            .HasColumnType("DECIMAL(18,2)")
            .HasColumnName("risk_limit");

        builder.Property(x => x.Role)
            .IsRequired()
            .HasDefaultValue(Role.Customer)
            .HasColumnName("role");

        // BaseEntity fields
        builder.Property(x => x.Id).HasColumnName("id");

        builder.Property(x => x.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd()
            .HasColumnName("created_at");

        builder.Property(x => x.UpdatedAt)
            .IsRequired(false)
            .HasColumnName("updated_at");

        builder.Property(x => x.IsDeleted)
            .HasDefaultValue(false)
            .HasColumnName("is_deleted");



        builder.HasMany(x => x.Cards)
            .WithOne(c => c.Customer)
            .HasForeignKey(c => c.CustomerId) // Düzeltildi
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Transactions)
            .WithOne(t => t.Customer)
            .HasForeignKey(t => t.CustomerId) // Düzeltildi
            .OnDelete(DeleteBehavior.Cascade);
    }
}