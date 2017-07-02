﻿using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using DataLayer.Enitities;

namespace DataLayer
{
    public class ApplicationUser : IdentityUser
    {
        public virtual UserProfile userDetail { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasRequired(c => c.photo)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Category>()
                .HasRequired(s => s.photo)
                .WithMany()
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductTranslation> ProductTranslations { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<SubjectTranslation> SubjectTranslations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryTranslation> CategoryTranslations { get; set; }
        public DbSet<Medium> Mediums { get; set; }
        public DbSet<MediumTranslation> MediumTranslations { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<MaterialTranslation> MaterialTranslations { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<StyleTranslation> StyleTranslations { get; set; }
        public DbSet<SiteParam> SiteParams { get; set; }
        public DbSet<SiteParamTranslation> SiteParamTranslations { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<SiteObjectParam> SiteObjectParams { get; set; }
        public DbSet<NavigationCategory> NavigationCategories { get; set; }
        public DbSet<NavigationCategoryFavStyle> NavigationCategoryFavStyles { get; set; }
        public DbSet<NavigationCategoryFavMedium> NavigationCategoryFavMediums { get; set; }
        public DbSet<NavigationCategorySubject> NavigationCategorySubjects { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<CollectionProduct> CollectionProduct { get; set; }
        public DbSet<PersonalInformation> PersonalInformations { get; set; }
        public DbSet<UserLink> UserLinks { get; set; }

    }
}