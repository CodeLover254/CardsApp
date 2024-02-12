using CardsApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CardsApp.Domain.EntityTypeConfigurations;

public class CardEntityTypeConfiguration: IEntityTypeConfiguration<Card>
{
    public void Configure(EntityTypeBuilder<Card> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.Property(x => x.Color).HasMaxLength(8);
        builder.Property(x => x.UserId).HasMaxLength(40);
        builder.Property(x => x.CardState).HasConversion<string>();
    }
}