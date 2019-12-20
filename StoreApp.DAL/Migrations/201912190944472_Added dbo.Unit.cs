namespace StoreApp.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddeddboUnit : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Units",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Products", "UnitId", c => c.Int(nullable: false));
            CreateIndex("dbo.Products", "UnitId");
            AddForeignKey("dbo.Products", "UnitId", "dbo.Units", "Id", cascadeDelete: false);
            DropColumn("dbo.Products", "Unit");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Unit", c => c.String());
            DropForeignKey("dbo.Products", "UnitId", "dbo.Units");
            DropIndex("dbo.Products", new[] { "UnitId" });
            DropColumn("dbo.Products", "UnitId");
            DropTable("dbo.Units");
        }
    }
}
