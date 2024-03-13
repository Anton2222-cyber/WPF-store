using Data.Entities;
using Data.Entities.Identity;
using Data.EntityTypeConfigurations;
using Data.EntityTypeConfigurations.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class DataContext(DbContextOptions<DataContext> options)
	: IdentityDbContext<User, Role, long, IdentityUserClaim<long>, UserRole, IdentityUserLogin<long>,
		IdentityRoleClaim<long>, IdentityUserToken<long>>(options) {

	public DbSet<Category> Categories { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<Image> Images { get; set; }
	public DbSet<BasketProduct> BasketProducts { get; set; }
	public DbSet<Order> Orders { get; set; }
	public DbSet<OrderedProduct> OrderedProducts { get; set; }
	public DbSet<OrderStatus> OrderStatuses { get; set; }
	public DbSet<PostOffice> PostOffices { get; set; }
	public DbSet<Settlement> Settlements { get; set; }
	public DbSet<Area> Areas { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder) {
		base.OnModelCreating(modelBuilder);

		new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
		new UserRoleEntityTypeConfiguration().Configure(modelBuilder.Entity<UserRole>());

		new CategoryEntityTypeConfiguration().Configure(modelBuilder.Entity<Category>());
		new ProductEntityTypeConfiguration().Configure(modelBuilder.Entity<Product>());
		new ImageEntityTypeConfiguration().Configure(modelBuilder.Entity<Image>());
		new BasketProductEntityTypeConfiguration().Configure(modelBuilder.Entity<BasketProduct>());
		new OrderEntityTypeConfiguration().Configure(modelBuilder.Entity<Order>());
		new OrderedProductEntityTypeConfiguration().Configure(modelBuilder.Entity<OrderedProduct>());
		new OrderStatusEntityTypeConfiguration().Configure(modelBuilder.Entity<OrderStatus>());
		new PostOfficeEntityTypeConfiguration().Configure(modelBuilder.Entity<PostOffice>());
		new SettlementEntityTypeConfiguration().Configure(modelBuilder.Entity<Settlement>());
		new AreaEntityTypeConfiguration().Configure(modelBuilder.Entity<Area>());
	}
}
