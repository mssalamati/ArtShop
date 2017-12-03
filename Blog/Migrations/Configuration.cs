namespace Blog.Migrations
{
    using Models;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<Blog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
        protected override void Seed(Blog.Models.ApplicationDbContext context)
        {
            var rm = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);
            var user = new ApplicationUser { UserName = "admin", Email = "adb.dehghan@gmail.com", PhoneNumber = "09213175268", userDetail = new UserProfile() };
            manager.Create(user, "Art123123");
        }
    }
}
