using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityTypeConfigurations;

internal class ImageEntityTypeConfiguration : IEntityTypeConfiguration<Image> {
	public void Configure(EntityTypeBuilder<Image> builder) {
		builder.ToTable("Images");

		builder.HasOne(i => i.Product)
			.WithMany(p => p.Images)
			.HasForeignKey(i => i.ProductId)
			.IsRequired();



		builder.Property(i => i.Name)
			.IsRequired()
			.HasMaxLength(100);
	}
}