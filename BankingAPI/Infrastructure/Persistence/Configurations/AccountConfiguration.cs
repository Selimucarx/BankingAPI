using BankingAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");

        builder.Property(x => x.AccountName)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnName("account_name");

        builder.Property(x => x.AccountNumber)
            .IsRequired()
            .HasMaxLength(16)
            .IsFixedLength()
            .HasColumnName("account_number");

        builder.HasIndex(x => x.AccountNumber).IsUnique();

        builder.Property(x => x.Iban)
            .IsRequired()
            .HasMaxLength(26)
            .HasColumnName("iban");

        builder.Property(x => x.Balance)
            .IsRequired()
            .HasDefaultValue(0.00)
            .HasColumnName("balance");

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true)
            .HasColumnName("is_active");


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


        // Relationships
        builder.HasMany(x => x.Cards)
            .WithOne(a => a.Account)
            .HasForeignKey(c => c.AccountId) // Düzeltildi
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(x => x.Transactions)
            .WithOne(a => a.Account)
            .HasForeignKey(t => t.AccountId) // Düzeltildi
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(x => x.Customer)
            .WithMany(c => c.Accounts)
            .HasForeignKey(a => a.CustomerId)
            .OnDelete(DeleteBehavior.Cascade); 
    }
}