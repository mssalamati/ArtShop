namespace DataLayer.Migrations
{
    using Enitities;
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
