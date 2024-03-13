using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.EntityTypeConfigurations;

internal class AreaEntityTypeConfiguration : IEntityTypeConfiguration<Area> {
	public void Configure(EntityTypeBuilder<Area> builder) {
		builder.ToTable("Areas");

		builder.Property(s => s.Name)
			.HasMaxLength(100)
			.IsRequired();
	}
}