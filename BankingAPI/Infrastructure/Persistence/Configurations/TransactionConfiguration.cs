using BankingAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("transactions");

        builder.Property(x => x.CardNumber)
            .IsRequired()
            .HasMaxLength(16)
            .IsFixedLength()
            .HasColumnName("card_number");

        builder.Property(x => x.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)")
            .HasColumnName("amount");

        builder.Property(x => x.Description)
            .HasMaxLength(100)
            .HasColumnName("description");

        builder.Property(x => x.SenderIbanNumber)
            .HasMaxLength(26)
            .IsFixedLength()
            .HasColumnName("sender_iban_number");

        builder.Property(x => x.ReceiverIbanNumber)
            .HasMaxLength(26)
            .IsFixedLength()
            .HasColumnName("receiver_iban_number");

        // Relationships
        builder.HasOne(x => x.Card)
            .WithMany(c => c.Transactions)
            .HasForeignKey(x => x.CardId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Account)
            .WithMany(a => a.Transactions)
            .HasForeignKey(x => x.AccountId);

        builder.HasOne(x => x.Customer)
            .WithMany(c => c.Transactions)
            .HasForeignKey(x => x.CustomerId);
    }
}