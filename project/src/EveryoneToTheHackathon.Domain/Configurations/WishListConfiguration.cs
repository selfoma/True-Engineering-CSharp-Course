using EveryoneToTheHackathon.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EveryoneToTheHackathon.Domain.Configurations;

public class WishListConfiguration : IEntityTypeConfiguration<WishList>
{
    public void Configure(EntityTypeBuilder<WishList> builder)
    {
        builder.HasKey(w => w.WishListId);

        builder
            .HasOne(w => w.HackathonEmployeeWishListMappings)
            .WithMany(m => m.WishLists)
            .HasForeignKey(w => w.MappingId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}