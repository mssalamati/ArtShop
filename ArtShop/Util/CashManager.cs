using ArtShop.Models;
using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataLayer.Extentions;
using Microsoft.AspNet.Identity;
using System.Configuration;
using DataLayer.Enitities;
using System.Globalization;
using System.Data.Entity;

namespace ArtShop.Util
{
    public class CashManager
    {
        public static CashManager Instance { get { if (_Instance == null) _Instance = new CashManager(); return _Instance; } }
        private static CashManager _Instance;
        private HomeIndexViewModel _Header = null;
        private List<Subject> _Subjects = null;
        private List<Medium> _Mediums = null;
        private List<Material> _Materials = null;
        private List<Style> _Styles = null;
        private List<Category> _Categories = null;
        private List<Pricethreshold> _Pricethreshold = null;     
        private List<NavigationCategory> cats;
        private List<Country> _countries = null;
        public CashManager()
        {
            cats = new List<NavigationCategory>();
        }

        public Dictionary<int, string> Subjects
        {
            get
            {
                if (_Subjects == null)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        _Subjects = db.Subjects.Include("Translations").ToList();
                    }
                }
                return _Subjects.ToDictionary(x => x.Id, y => y.Current().Name);
            }
        }

        public Dictionary<int, string> Mediums
        {
            get
            {
                if (_Mediums == null)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        _Mediums = db.Mediums.Where(x => x.AddedByAdmin).Include("Translations").ToList();
                    }
                }
                return _Mediums.ToDictionary(x => x.Id, y => y.Current().Name);
            }
        }

        public Dictionary<int, string> Materials
        {
            get
            {
                if (_Materials == null)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        _Materials = db.Materials.Where(x => x.AddedByAdmin).Include("Translations").ToList();
                    }
                }
                return _Materials.ToDictionary(x => x.Id, y => y.Current().Name);
            }
        }

        public Dictionary<int, string> Styles
        {
            get
            {
                if (_Styles == null)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        _Styles = db.Styles.Where(x => x.AddedByAdmin).Include("Translations").ToList();
                    }
                }
                return _Styles.ToDictionary(x => x.Id, y => y.Current().Name);
            }
        }

        public List<CategoryViewModel> Categories
        {
            get
            {
                if (_Categories == null)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        _Categories = db.Categories.Include("Translations").Include("photo").ToList();
                    }
                }
                return _Categories.Select(x => new CategoryViewModel
                {
                    id = x.Id,
                    photo = ConfigurationManager.AppSettings["FileUrl"] + x.photo.Path,
                    name = x.Current().Name
                }).ToList();
            }
        }

        public List<Pricethreshold> Pricethresholds
        {
            get
            {
                if (_Pricethreshold == null)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        _Pricethreshold = db.Pricethresholds.Include("Translations").ToList();
                    }
                }
                return _Pricethreshold.Select(x => new Pricethreshold
                {
                    Id = x.Id,
                    Name = x.Current().Name,
                    min = x.min,
                    max = x.max,
                }).ToList();
            }
        }

        public Dictionary<int, string> Countries
        {
            get
            {
                if (_countries == null)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        _countries = db.Countries.Include("Translations").ToList();
                    }
                }
                return _countries.ToDictionary(x => x.Id, y => y.Current().Name);
            }
        }

        public HomeIndexViewModel Header
        {
            get
            {
                using (ApplicationDbContext db = new ApplicationDbContext())
                {
                    if (_Header == null)
                    {
                        var SiteObjectParams = db.SiteObjectParams.AsQueryable().FirstOrDefault();
                        cats = SiteObjectParams.Navigations.ToList();
                    }

                    string currentCultureName = CultureInfo.CurrentCulture.Name.Substring(0, 2);
                    _Header = new HomeIndexViewModel();
                    _Header.Navigation = cats.Select(x => new IdNameViewModel()
                    {
                        Id = x.categoryId,
                        Name = x.category.Translations.SingleOrDefault(t => t.language.Code == currentCultureName).Name,
                        Photo = ConfigurationManager.AppSettings["FileUrl"] + x.category.photo.Path,
                        FavMediums = x.FavMediums.Select(fm => new IdNameViewModel()
                        {
                            Id = fm.mediumId,
                            Name = fm.medium.Translations.SingleOrDefault(t => t.language.Code == currentCultureName).Name
                        }).ToList(),
                        FavStyles = x.FavStyles.Select(fm => new IdNameViewModel()
                        {
                            Id = fm.styleId,
                            Name = fm.style.Translations.SingleOrDefault(t => t.language.Code == currentCultureName).Name
                        }).ToList(),
                        FavSubjects = x.FavSubjects.Select(fm => new IdNameViewModel()
                        {
                            Id = fm.subjectId,
                            Name = fm.subject.Translations.SingleOrDefault(t => t.language.Code == currentCultureName).Name
                        }).ToList()
                    }).ToList();
                    return _Header;
                }
            }
        }
 
    }
}