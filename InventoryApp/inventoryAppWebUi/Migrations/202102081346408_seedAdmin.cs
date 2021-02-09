namespace inventoryAppWebUi.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedAdmin : DbMigration
    {
        public override void Up()
        {
            var userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>());
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
            var user = new IdentityUser()
            {
                UserName = "Admin@admin.com",
                Email = "Admin@admin.com",
                EmailConfirmed = true,
                // TwoFactorEnabled = true,
            };

            var result = userManager.CreateAsync(user, "Admin1234_");

            if (result.IsCompleted)
            {
                if (result.Result.Succeeded)
                {
                    var admin = new IdentityRole("Admin");
                    var roleResult = roleManager.CreateAsync(admin);

                    if (roleResult.IsCompleted)
                    {
                        if (roleResult.Result.Succeeded)
                        {
                            userManager.AddToRoleAsync(user.Id, "Admin");
                        }
                    }
                }
            }

        }

        public override void Down()
        {
        }
    }
}
