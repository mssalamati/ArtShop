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

namespace ArtShop.Util
{
    public class CashManager
    {
        public static CashManager Instance { get { if (_Instance == null) _Instance = new CashManager(); return _Instance; } }
        public static CashManager _Instance;

        private List<NavigationCategory> cats;

        public HomeIndexViewModel _Header = null;

        ApplicationDbContext db = new ApplicationDbContext();
        public HomeIndexViewModel Header
        {
            get
            {
                if (_Header == null)
                {

                    var SiteObjectParams = db.SiteObjectParams.AsQueryable().FirstOrDefault();
                    cats = SiteObjectParams.Navigations.ToList();

                }

                _Header = new HomeIndexViewModel();
                _Header.Navigation = cats.Select(x => new IdNameViewModel()
                {
                    Id = x.categoryId,
                    Name = x.category.Current().Name,
                    Photo = ConfigurationManager.AppSettings["FileUrl"] + x.category.photo.Path,
                    FavMediums = x.FavMediums.Select(fm => new IdNameViewModel() { Id = fm.mediumId, Name = fm.medium.Current().Name }).ToList(),
                    FavStyles = x.FavStyles.Select(fm => new IdNameViewModel() { Id = fm.styleId, Name = fm.style.Current().Name }).ToList(),
                    FavSubjects = x.FavSubjects.Select(fm => new IdNameViewModel() { Id = fm.subjectId, Name = fm.subject.Current().Name }).ToList()
                }).ToList();
                return _Header;
            }
        }

        public CashManager()
        {
            cats = new List<NavigationCategory>();
        }
    }
}