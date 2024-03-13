namespace LocalData
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Value = c.String(maxLength: 2147483647),
                    })
                .PrimaryKey(t => t.Name);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Settings");
        }
    }
}
