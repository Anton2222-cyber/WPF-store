using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityTypeConfigurations;

internal class OrderedProductEntityTypeConfiguration : IEntityTypeConfiguration<OrderedProduct> {
	public void Configure(EntityTypeBuilder<OrderedProduct> builder) {
		builder.ToTable("OrderedProducts");

		builder.HasOne(op => op.Product)
			.WithMany(p => p.OrderedProducts)
			.HasForeignKey(op => op.ProductId)
			.IsRequired();

		builder.HasOne(op => op.Order)
			.WithMany(o => o.OrderedProducts)
			.HasForeignKey(op => op.OrderId)
			.IsRequired();

		builder.Property(op => op.UnitPrice)
			.HasAnnotation("Range", new[] { 0, double.MaxValue })
			.IsRequired();

		builder.Property(op => op.Quantity)
			.HasAnnotation("Range", new[] { 1, int.MaxValue })
			.IsRequired();
	}
}
