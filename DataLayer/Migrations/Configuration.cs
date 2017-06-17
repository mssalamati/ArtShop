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
            var user = new ApplicationUser { UserName = "superadmin", Email = "ms.salamati@gmail.com", PhoneNumber = "9374641231", userDetail = new UserProfile() };
            manager.Create(user, "Art123");

            context.SiteParams.AddOrUpdate(x => x.Name,
                new Enitities.SiteParam() { Name = "slider H1" },
                new Enitities.SiteParam() { Name = "slider H2" },
                new Enitities.SiteParam() { Name = "slider Button Text" },
                new Enitities.SiteParam() { Name = "slider P" },
                new Enitities.SiteParam() { Name = "slider Button Url" },
                new Enitities.SiteParam() { Name = "Selected Art 1" },
                new Enitities.SiteParam() { Name = "Selected Art 2" },
                new Enitities.SiteParam() { Name = "Selected Art 3" });

            var obj = context.SiteObjectParams.FirstOrDefault();
            if (obj == null)
            {
                context.SiteObjectParams.Add(new SiteObjectParam() { SiteName = "ARTSHOP" });
            }
        }
    }
}
