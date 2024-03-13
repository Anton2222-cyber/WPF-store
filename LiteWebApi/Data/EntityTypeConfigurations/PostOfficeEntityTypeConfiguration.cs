using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityTypeConfigurations;

internal class PostOfficeEntityTypeConfiguration : IEntityTypeConfiguration<PostOffice> {
	public void Configure(EntityTypeBuilder<PostOffice> builder) {
		builder.ToTable("PostOffices");

		builder.Property(po => po.Name)
			.HasMaxLength(200)
			.IsRequired();

		builder.HasOne(po => po.Settlement)
			.WithMany(s => s.PostOffices)
			.HasForeignKey(po => po.SettlementId)
			.IsRequired();
	}
}
