namespace DataLayer.Migrations
{
    using Enitities;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataLayer.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(DataLayer.ApplicationDbContext context)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);

            //add admin role
            if (!rm.RoleExistsAsync("Superadmin").Result)
            {
                IdentityRole role = new IdentityRole("Superadmin");
                IdentityResult roleResult = rm.CreateAsync(role).Result;
            }

            if (!rm.RoleExistsAsync("Administrator").Result)
            {
                IdentityRole role = new IdentityRole("Administrator");
                IdentityResult roleResult = rm.CreateAsync(role).Result;
            }

            if (!rm.RoleExistsAsync("Manager").Result)
            {
                IdentityRole role = new IdentityRole("Manager");
                IdentityResult roleResult = rm.CreateAsync(role).Result;
            }

            if (manager.FindByNameAsync("superadmin").Result == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = "adb.dehghan@gmail.com",
                    UserName = "superadmin",
                    PhoneNumber = "09213175268",
                    adminDetail = new AdminProfile(),
                    userDetail = new UserProfile()
                };
                IdentityResult result = manager.CreateAsync(user, "Art123").Result;
                if (result.Succeeded)
                    manager.AddToRoleAsync(user.Id, "Superadmin").Wait();
            }

            if (manager.FindByNameAsync("admin").Result == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = "admin@artiscovery.com",
                    UserName = "admin",
                    PhoneNumber = "09213175268",
                    adminDetail = new AdminProfile(),
                    userDetail = new UserProfile()
                };
                IdentityResult result = manager.CreateAsync(user, "Art123456").Result;
                if (result.Succeeded)
                    manager.AddToRoleAsync(user.Id, "Administrator").Wait();
            }

            if (manager.FindByNameAsync("manager").Result == null)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = "manager@artiscovery.com",
                    UserName = "manager",
                    PhoneNumber = "09213175268",
                    adminDetail = new AdminProfile(),
                    userDetail = new UserProfile()
                };
                IdentityResult result = manager.CreateAsync(user, "Art1234").Result;
                if (result.Succeeded)
                    manager.AddToRoleAsync(user.Id, "Manager").Wait();
            }

            //var user = new ApplicationUser { UserName = "superadmin", Email = "ms.salamati@gmail.com", PhoneNumber = "9374641231", adminDetail = new AdminProfile(), userDetail = new UserProfile() };
            //manager.Create(user, "Art123");

            //manager.AddToRoleAsync(user.Id, "Administrator").Wait();

            context.SiteParams.AddOrUpdate(x => x.Name,
                new SiteParam() { Name = "Facebook" },
                new SiteParam() { Name = "Telegram" },
                new SiteParam() { Name = "Twiter" },
                new SiteParam() { Name = "Pintrest" },
                new SiteParam() { Name = "Instagram" },
                new SiteParam() { Name = "Youtube" });

            context.SettingValues.AddOrUpdate(x => x.siteName, new SettingValue() { siteName = "Artiscovery", UpdateDate = DateTime.Now, UpdaterUser = "soroosh" });
        }
    }
}
