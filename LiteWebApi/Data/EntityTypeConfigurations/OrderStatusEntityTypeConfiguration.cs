using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityTypeConfigurations;

internal class OrderStatusEntityTypeConfiguration : IEntityTypeConfiguration<OrderStatus> {
	public void Configure(EntityTypeBuilder<OrderStatus> builder) {
		builder.ToTable("OrderStatuses");

		builder.Property(os => os.Status)
			.HasMaxLength(200)
			.IsRequired();
	}
}