using BankingAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class CardConfiguration : IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.ToTable("cards");

        builder.Property(x => x.CardNumber)
            .IsRequired(false)
            .HasMaxLength(16)
            .IsFixedLength()
            .HasColumnName("card_number");


        builder.Property(x => x.BankName)
            .IsRequired(false)
            .HasColumnName("bank_name");

        builder.Property(x => x.Cvv)
            .IsRequired(false)
            .HasMaxLength(3)
            .IsFixedLength()
            .HasColumnName("cvv");

        builder.Property(x => x.LimitAmount)
            .IsRequired(false)
            .HasColumnType("decimal(18,2)")
            .HasColumnName("limit_amount");

        builder.Property(x => x.AvailableLimit)
            .IsRequired(false)
            .HasColumnType("decimal(18,2)")
            .HasColumnName("available_limit");

        builder.Property(x => x.IsActive)
            .IsRequired(false)
            .HasDefaultValue(true)
            .HasColumnName("is_active");

        // Base entity
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
        builder.HasMany(x => x.Transactions)
            .WithOne(a => a.Card)
            .HasForeignKey(a => a.CardId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasOne(x => x.Account)
            .WithMany(a => a.Cards)
            .HasForeignKey(x => x.AccountId);

        builder.HasOne(x => x.Customer)
            .WithMany(c => c.Cards)
            .HasForeignKey(x => x.CustomerId);
    }
}