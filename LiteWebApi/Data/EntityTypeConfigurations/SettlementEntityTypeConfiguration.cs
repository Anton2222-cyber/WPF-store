using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityTypeConfigurations;

internal class SettlementEntityTypeConfiguration : IEntityTypeConfiguration<Settlement> {
	public void Configure(EntityTypeBuilder<Settlement> builder) {
		builder.ToTable("Settlements");

		builder.Property(s => s.Name)
			.HasMaxLength(100)
			.IsRequired();

		builder.HasOne(s => s.Area)
			.WithMany(a => a.Settlements)
			.HasForeignKey(s => s.AreaId)
			.IsRequired();
	}
}
