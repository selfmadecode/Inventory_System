using System.Collections.Generic;
using inventoryAppDomain.Entities;
using inventoryAppDomain.IdentityEntities;

namespace inventoryAppWebUi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seedCategory : DbMigration
    {
        public override void Up()
        {
            IList<DrugCategory> drugCategories = new List<DrugCategory>();

            drugCategories.Add(new DrugCategory() { CategoryName = "Syrup" });
            drugCategories.Add(new DrugCategory() { CategoryName = "Tablet" });
            drugCategories.Add(new DrugCategory() { CategoryName = "Injectable" });
            drugCategories.Add(new DrugCategory() { CategoryName = "Others" });

            ApplicationDbContext context = new ApplicationDbContext();

            context.DrugCategories.AddRange(drugCategories);

            context.SaveChanges();
        }
        
        public override void Down()
        {
        }
    }
}
