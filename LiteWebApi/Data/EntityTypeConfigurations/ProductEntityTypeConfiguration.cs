using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityTypeConfigurations;

internal class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product> {
	public void Configure(EntityTypeBuilder<Product> builder) {
		builder.ToTable("Products");

		builder.HasOne(p => p.Category)
			.WithMany(c => c.Products)
			.HasForeignKey(p => p.CategoryId)
			.IsRequired();

		builder.HasMany(p => p.Images)
			.WithOne(i => i.Product)
			.HasForeignKey(i => i.ProductId)
			.IsRequired();



		builder.Property(p => p.Name)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(p => p.Description)
			.HasMaxLength(4000);
	}
}