namespace StoreApp.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RenameHomeAdresstoHomeAddressindboUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "HomeAddress", c => c.String());
            DropColumn("dbo.Users", "HomeAdress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "HomeAdress", c => c.String());
            DropColumn("dbo.Users", "HomeAddress");
        }
    }
}
