using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityTypeConfigurations;

internal class OrderEntityTypeConfiguration : IEntityTypeConfiguration<Order> {
	public void Configure(EntityTypeBuilder<Order> builder) {
		builder.ToTable("Orders");

		builder.HasOne(o => o.User)
			.WithMany(u => u.Orders)
			.HasForeignKey(o => o.UserId)
			.IsRequired();

		builder.HasOne(o => o.Status)
			.WithMany()
			.HasForeignKey(o => o.StatusId)
			.IsRequired();

		builder.HasMany(o => o.OrderedProducts)
			.WithOne(op => op.Order)
			.HasForeignKey(op => op.OrderId)
			.IsRequired();

		builder.HasOne(o => o.PostOffice)
			.WithMany()
			.HasForeignKey(o => o.PostOfficeId)
			.IsRequired();
	}
}
