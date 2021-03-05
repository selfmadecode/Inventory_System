namespace inventoryAppWebUi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NotificationsUpdate : DbMigration
    {
        public override void Up()
        {
        }
        
        public override void Down()
        {
            DropColumn("dbo.Notifications", "Title");
            DropColumn("dbo.Suppliers", "Status");
        }
    }
}
