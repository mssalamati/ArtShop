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
        private List<PrintMaterial> _PrintMaterial = null;
        private List<footerCell> _footer = null;

        private List<ProductFrameColor> _ProductFrameColor = null;
        private List<ProductFrameMaterial> _ProductFrameMaterial = null;
        private List<ProductFrameType> _ProductFrameType = null;

        public CashManager()
        {
            cats = new List<NavigationCategory>();
        }

        public static void resete()
        {
            _Instance = null;
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
                return _Subjects.Select(x => new { id = x.Id, name = x.Current().Name }).OrderBy(x => x.name).ToDictionary(x => x.id, y => y.name);
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
                return _Mediums.Select(x => new { id = x.Id, name = x.Current().Name }).OrderBy(x => x.name).ToDictionary(x => x.id, y => y.name);
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
                return _Materials.Select(x => new { id = x.Id, name = x.Current().Name }).OrderBy(x => x.name).ToDictionary(x => x.id, y => y.name);
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
                return _Styles.Select(x => new { id = x.Id, name = x.Current().Name }).OrderBy(x => x.name).ToDictionary(x => x.id, y => y.name);
            }
        }

        public ProductFrameOptions productFrameOptions
        {
            get
            {
                if (_ProductFrameColor == null && _ProductFrameMaterial == null && _ProductFrameType == null)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        _ProductFrameColor = db.ProductFrameColors.Include("Translations").ToList();
                        _ProductFrameMaterial = db.ProductFrameMaterials.Include("Translations").ToList();
                        _ProductFrameType = db.ProductFrameTypes.Include("Translations").ToList();
                    }
                }
                return new ProductFrameOptions()
                {
                    ProductFrameColors = _ProductFrameColor.ToDictionary(x => x.Id, y => y.Current().Name),
                    ProductFrameMaterials = _ProductFrameMaterial.ToDictionary(x => x.Id, y => y.Current().Name),
                    ProductFrameTypes = _ProductFrameType.ToDictionary(x => x.Id, y => y.Current().Name)
                };
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
                }).OrderBy(a => a.name).ToList();
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
                return _countries.Select(x => new { id = x.Id, name = x.Current().Name }).OrderBy(x => x.name).ToDictionary(x => x.id, y => y.name);
            }
        }

        public List<PrintMaterial> PrintMaterial
        {
            get
            {
                if (_PrintMaterial == null)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        _PrintMaterial = db.PrintMaterials.Select(x => new
                        {
                            id = x.Id,
                            Translations = x.Translations.ToList(),
                            PrintSizes = x.PrintSizes.Select(y => new
                            {
                                id = y.Id,
                                price = y.price,
                                Width = y.Width,
                                Height = y.Height,
                                Translations = y.Translations.ToList(),
                                PrintFrames = y.PrintFrames.Select(z => new
                                {
                                    id = z.Id,
                                    Translations = z.Translations.ToList(),
                                    price = z.price,
                                    color = z.color,
                                    size = z.size
                                }).ToList()
                            }).ToList()
                        }).ToList().Select(x => new PrintMaterial()
                        {
                            Id = x.id,
                            Translations = x.Translations.ToList(),
                            PrintSizes = x.PrintSizes.Select(y => new PrintSize()
                            {
                                Id = y.id,
                                price = y.price,
                                Width = y.Width,
                                Height = y.Height,
                                Translations = y.Translations.ToList(),
                                PrintFrames = y.PrintFrames.Select(z => new PrintFrame()
                                {
                                    Id = z.id,
                                    Translations = z.Translations.ToList(),
                                    price = z.price,
                                    color = z.color,
                                    size = z.size
                                }).ToList()
                            }).ToList()
                        }).ToList();
                    }
                }
                return _PrintMaterial;
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
                        cats = db.NavigationCategories.ToList();
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

        public List<footerCell> Footer
        {
            get
            {
                if (_footer == null)
                {
                    using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        _footer = db.footerCells.Include("Translations").ToList();
                    }
                }
                return _footer.ToList();
            }
        }

        public decimal GetShipingDHL(int country)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var region = db.Countries.Find(country).region;
                switch (region)
                {
                    case CountryRegion.A:
                        return 1000;
                    case CountryRegion.B:
                        return 1000;
                    case CountryRegion.C:
                        return 1000;
                    case CountryRegion.D:
                        return 1000;
                    case CountryRegion.E:
                        return 1000;
                    case CountryRegion.F:
                        return 1000;
                    case CountryRegion.G:
                        return 1000;
                    case CountryRegion.H:
                        return 1000;
                    case CountryRegion.I:
                        return 1000;
                    case CountryRegion.J:
                        return 1000;                    
                    default:
                        return 0;
                }
            }
        }
    }
}