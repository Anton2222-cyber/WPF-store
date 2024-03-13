using LocalData.Context;
using System.Data.Entity.Migrations;
using System.Data.SQLite.EF6.Migrations;

namespace LocalData {
	internal sealed class Configuration : DbMigrationsConfiguration<LocalDataContext> {
		public Configuration() {
			AutomaticMigrationsEnabled = false;
			SetSqlGenerator("System.Data.SQLite", new SQLiteMigrationSqlGenerator());
		}

		protected override void Seed(LocalDataContext context) {

		}
	}
}
