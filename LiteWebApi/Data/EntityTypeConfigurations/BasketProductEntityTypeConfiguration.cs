using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityTypeConfigurations;

internal class BasketProductEntityTypeConfiguration : IEntityTypeConfiguration<BasketProduct> {
	public void Configure(EntityTypeBuilder<BasketProduct> builder) {
		builder.ToTable("BasketProducts");

		builder.HasOne(bp => bp.User)
			.WithMany(u => u.BasketProducts)
			.HasForeignKey(bp => bp.UserId)
			.IsRequired();

		builder.HasOne(bp => bp.Product)
			.WithMany(p => p.BasketProducts)
			.HasForeignKey(bp => bp.ProductId)
			.IsRequired();

		builder.Property(bp => bp.Quantity)
			.HasAnnotation("Range", new[] { 1, int.MaxValue })
			.IsRequired();
	}
}
