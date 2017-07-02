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


        private HomeIndexViewModel _Header = null;
        private Dictionary<int, string> _Subjects = null;
        private Dictionary<int, string> _Mediums = null;
        private Dictionary<int, string> _Materials = null;
        private Dictionary<int, string> _Styles = null;
        private List<CategoryViewModel> _Categories = null;
        private static CashManager _Instance;
        private List<NavigationCategory> cats;

        public Dictionary<int, string> Subjects
        {
            get
            {
                if (_Subjects == null)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        _Subjects = db.Subjects.Include("Translations").ToList().ToDictionary(x => x.Id, y => y.Current().Name);
                    }
                }
                return _Subjects;
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
                        _Mediums = db.Mediums.Where(x => x.AddedByAdmin).Include("Translations").ToList().ToDictionary(x => x.Id, y => y.Current().Name);
                    }
                }
                return _Mediums;
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
                        _Materials = db.Materials.Where(x => x.AddedByAdmin).Include("Translations").ToList().ToDictionary(x => x.Id, y => y.Current().Name);
                    }
                }
                return _Materials;
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
                        _Styles = db.Styles.Where(x => x.AddedByAdmin).Include("Translations").ToList().ToDictionary(x => x.Id, y => y.Current().Name);
                    }
                }
                return _Styles;
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
                        _Categories = db.Categories.Include("Translations").ToList().Select(x => new CategoryViewModel
                        {
                            id = x.Id,
                            photo = ConfigurationManager.AppSettings["FileUrl"] + x.photo.Path,
                            name = x.Current().Name
                        }).ToList();
                    }
                }
                return _Categories;
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

        public CashManager()
        {
            cats = new List<NavigationCategory>();
        }
    }
}