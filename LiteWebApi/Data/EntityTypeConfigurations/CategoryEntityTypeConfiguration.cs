using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityTypeConfigurations;

internal class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<Category> {
	public void Configure(EntityTypeBuilder<Category> builder) {
		builder.ToTable("Categories");

		builder.HasMany(c => c.Products)
			.WithOne(p => p.Category)
			.HasForeignKey(p => p.CategoryId)
			.IsRequired();



		builder.Property(c => c.Name)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(c => c.Image)
			.IsRequired()
			.HasMaxLength(300);

		builder.Property(c => c.Description)
			.HasMaxLength(4000);
	}
}