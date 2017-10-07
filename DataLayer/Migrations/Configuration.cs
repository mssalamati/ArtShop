namespace DataLayer.Migrations
{
    using Enitities;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

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
            var user = new ApplicationUser { UserName = "superadmin", Email = "ms.salamati@gmail.com", PhoneNumber = "9374641231", adminDetail = new AdminProfile(), userDetail = new UserProfile() };
            manager.Create(user, "Art123");

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
