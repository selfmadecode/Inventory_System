namespace inventoryAppWebUi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationsUpdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Suppliers", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.Notifications", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "Title");
            DropColumn("dbo.Suppliers", "Status");
        }
    }
}
