using LocalData.Entities;
using System.Data.Entity;

namespace LocalData.Context {
	public class LocalDataContext : DbContext {
		public DbSet<Setting> Settings { get; set; }

		public LocalDataContext() : base("name=SQLiteConnectionString") {
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<LocalDataContext, Configuration>());
		}
	}
}
