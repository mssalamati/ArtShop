using DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http.Cors;
using DataLayer.Extentions;
using System.Web.Http;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using MobileApi.Models;
using System.Data.Entity.Validation;
using DataLayer.Enitities;
using System.Web;

namespace MobileApi.Controllers
{
    [EnableCors("*", "*", "GET,POST")]
    [RoutePrefix("api/Share")]
    public class ShareController : ApiController
    {
        ApplicationDbContext db = new ApplicationDbContext();
        internal JsonMediaTypeFormatter formatter = MJsonMaker();
        private static Func<JsonMediaTypeFormatter> MJsonMaker = () =>
        {
            var formatter = new JsonMediaTypeFormatter();
            var json = formatter.SerializerSettings;
            json.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
            json.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Utc;
            json.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
            json.Formatting = Newtonsoft.Json.Formatting.Indented;
            json.ContractResolver = new CamelCasePropertyNamesContractResolver();
            json.Culture = new CultureInfo("it-IT");
            return formatter;
        };

        [Route("getSiteParams")]
        public HttpResponseMessage getSiteParams(string language = "en")
        {
            var Categories = db.Categories.Select(x => new
            {
                id = x.Id,
                name = x.Translations.Any(t => t.languageId == language) ?
                 x.Translations.FirstOrDefault(t => t.languageId == language).Name : string.Empty,
                picturePath = "https://admin.artiscovery.com/" + x.photo.Path
            });
            var Styles = db.Styles.Where(x => x.AddedByAdmin).Select(x => new
            {
                id = x.Id,
                name = x.Translations.Any(t => t.languageId == language) ?
               x.Translations.FirstOrDefault(t => t.languageId == language).Name : string.Empty,
            });
            var Subjects = db.Subjects.Select(x => new
            {
                id = x.Id,
                name = x.Translations.Any(t => t.languageId == language) ?
                 x.Translations.FirstOrDefault(t => t.languageId == language).Name : string.Empty,
            });
            var Mediums = db.Mediums.Where(x => x.AddedByAdmin).Select(x => new
            {
                id = x.Id,
                name = x.Translations.Any(t => t.languageId == language) ?
                 x.Translations.FirstOrDefault(t => t.languageId == language).Name : string.Empty,
            });
            var Pricelists = db.Pricethresholds.Select(x => new
            {
                id = x.Id,
                name = x.Translations.Any(t => t.languageId == language) ?
                 x.Translations.FirstOrDefault(t => t.languageId == language).Name : string.Empty,
            });
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                Pricelists = Pricelists,
                Mediums = Mediums,
                Subjects = Subjects,
                Styles = Styles,
                Categories = Categories
            }, formatter);
        }

        [Route("getMainPages")]
        public HttpResponseMessage getMainPages(string language = "en")
        {
            var thirdpictest = db.Products.Take(3).Select(x => x.Sqphoto.Path).ToList();
            var fakedata = new List<object>()
            {
                new { id = 1, title = "پر فروش ها", photo = thirdpictest },
                new { id = 2, title = "ارزان ترین ها", photo = thirdpictest },
                new { id = 3, title = "عکاسی", photo = thirdpictest },
                new { id = 4, title = "نقاشی", photo = thirdpictest }
            };
            return Request.CreateResponse(HttpStatusCode.OK, fakedata, formatter);
        }

        [Route("getMainPageItems")]
        public HttpResponseMessage getMainPageItems(int id, int page = 1)
        {
            var products = db.Products.OrderBy(x => x.CreateDate);
            var t = products.Count();
            var result = products.Skip((page - 1) * 10).Take(10).Select(x => new
            {
                id = x.Id,
                title = x.Title,
                photo = x.Widephoto.Path,
                author = x.user.FirstName,
                price = x.Price
            }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, new { total = t, data = result }, formatter);
        }

        [Route("getCountries")]
        public HttpResponseMessage getCountries(string language = "en")
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                db.Countries.Include("Translations").Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Translations.FirstOrDefault(s => s.languageId == language).Name
                }), formatter);
        }

        [Route("getLanguages")]
        public HttpResponseMessage getLanguages()
        {
            return Request.CreateResponse(HttpStatusCode.OK,
                db.Languages.Select(x => new { x.Code, x.Name }), formatter);
        }

        [Route("getProduct")]
        public HttpResponseMessage getProduct(int id)
        {
            var product = db.Products.Find(id);
            return Request.CreateResponse(HttpStatusCode.OK, product.tojason(), formatter);
        }

        [HttpGet, Route("Search")]
        public HttpResponseMessage Search(int CategoryId = 0, int StyleId = 0, int SubjectId = 0, int MediumId = 0, int PriceListId = 0, int page = 1)
        {
            int pageSize = 10;
            var price_cash = db.Pricethresholds.SingleOrDefault(x => x.Id == PriceListId);
            var p = db.Products.OrderByDescending(x => x.CreateDate).AsQueryable();
            p = p.Where(x => CategoryId == 0 || x.categoryId == CategoryId).AsQueryable();
            p = p.Where(x => StyleId == 0 || x.Styles.FirstOrDefault(y => y.Id == StyleId) != null);
            p = p.Where(x => SubjectId == 0 || x.subjectId == SubjectId).AsQueryable();
            p = p.Where(x => MediumId == 0 || x.Mediums.FirstOrDefault(y => y.Id == MediumId) != null);
            if (price_cash != null && price_cash.max.HasValue)
                p = p.Where(x => x.Price < price_cash.max.Value);
            if (price_cash != null && price_cash.min.HasValue)
                p = p.Where(x => x.Price >= price_cash.min.Value && x.Price > 0);
            var count = p.Count();
            page = Math.Min(page, (int)Math.Ceiling((float)count / (float)pageSize));
            page = Math.Max(1, page);
            p = p.Skip((page - 1) * pageSize).Take(pageSize);
            var result = p.Select(x => new
            {
                Id = x.Id,
                photo = x.photo.Path,
                Title = x.Title,
                Description = x.Description,
                Author = x.user.FirstName + " " + x.user.LastName,
                country = x.user.countryId,
                Price = x.Price,
                isForSale = x.ISOrginalForSale,
                status = x.Status
            }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                page = page,
                count = count,
                pageSize = pageSize,
                result = result
            }, formatter);
        }

        [Authorize,HttpPost, Route("addToFavorit")]
        public HttpResponseMessage addToFavorit(favMV model)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            if (profile.Favorits.Any(x => x.productId == model.productId))
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    message = "product exist"
                }, formatter);
            else
                profile.Favorits.Add(new DataLayer.Enitities.Favorit() { productId = model.productId });
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                success = true,
                message = ""
            }, formatter);
        }

        [Authorize, HttpPost, Route("removeFromFavorti")]
        public HttpResponseMessage removeFromFavorti(favMV model)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            if (profile.Favorits.Any(x => x.productId == model.productId))
                profile.Favorits.Remove(profile.Favorits.First(x => x.productId == model.productId));
            else
                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    success = false,
                    message = "product Not exist in favorit list"
                }, formatter);
            db.SaveChanges();
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                success = true,
                message = ""
            }, formatter);
        }

        [Authorize, HttpPost, Route("Upload")]
        public async Task<HttpResponseMessage> Upload(UploadViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var error = new
                {
                    message = "The request is invalid.",
                    error = ModelState.Values.SelectMany(e => e.Errors.Select(er => er.ErrorMessage))
                };
                return Request.CreateResponse(HttpStatusCode.BadRequest, error);
            }
            var res = await uploadnow(model);
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                success = res.success,
                message = res.error,
                id = res.id
            }, formatter);
        }

        public class favMV { public int productId { get; set; } public string language { get; set; } }
        private async Task<upoadNowResult> uploadnow(UploadViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            var iserver = getAvaibleServer();
            var iserverPath = "https://" + iserver.Host + "/upload/upload";
            try
            {
                //upload picture
                var UploadResult = await UploadImage(model.Image, iserver.Id);
                if (!UploadResult.result)
                    return new upoadNowResult(0, "Image Server Upload Error: " + UploadResult.data, false);
                
                //reize picture
                var ResizeResult = await resize(new ResizeViewModel()
                {
                    image = UploadResult.data,
                    square_height = model.SqrResizeRect.height,
                    square_width = model.SqrResizeRect.width,
                    square_x = model.SqrResizeRect.x,
                    square_y = model.SqrResizeRect.y,
                    wide_height = model.WideResizeRect.height,
                    wide_width = model.WideResizeRect.width,
                    wide_x = model.WideResizeRect.x,
                    wide_y = model.WideResizeRect.y,
                }, iserver.Id);
                if (!ResizeResult.result)
                    return new upoadNowResult(0, "Image Server resize Error: " + ResizeResult.error, false);

                //define value
                var widepath = ResizeResult.WideFullPath;
                var sqpath = ResizeResult.SqureFullPath;
                var orginalpic = UploadResult.data;
                var categoryId = model.categoryId;
                var subjectId = model.SubjectId;
                int img_width = UploadResult.width;
                int img_height = UploadResult.height;
                string Mediums = model.mediums;
                int[] Materials = (int[])model.materials;
                string Styles = model.styles;

                var medumsList = Mediums.Split(',');
                var stylelist = Styles.Split(',');

                var product = new Product()
                {
                    photo = new Photo() { Path = orginalpic, width = img_width, Height = img_height },
                    Widephoto = new Photo() { Path = widepath },
                    Sqphoto = new Photo() { Path = sqpath },
                    Title = model.Title,
                    Description = model.Description,
                    Price = (int)model.Price,
                    ISOrginalForSale = model.isforsale,
                    AllEntity = model.LimitOf,
                    ArtCreatedYear = model.createDate,
                    avaible = model.Limitform,
                    Depth = model.Depth,
                    Height = model.Height,
                    Width = model.Width,
                    IsPrintAvaibled = false,
                    Keywords = model.keywords,
                    categoryId = categoryId,
                    subjectId = subjectId,
                    TotalWeight = model.weight,
                    Status = model.isforsale ? ProductStatus.forSale : ProductStatus.NotForSale
                };
                product.Materials = new List<Material>();
                product.Styles = new List<Style>();
                product.Mediums = new List<Medium>();

                foreach (var item in Materials)
                    product.Materials.Add(db.Materials.FirstOrDefault(x => item == x.Id));
                foreach (var item in stylelist)
                {
                    var temp = db.StyleTranslations.FirstOrDefault(x => x.Name == item);
                    if (temp != null)
                        product.Styles.Add(temp.style);
                }

                foreach (var item in medumsList)
                {
                    var temp = db.MediumTranslations.FirstOrDefault(x => x.Name == item);
                    if (temp != null)
                        product.Mediums.Add(temp.medium);
                }

                profile.Products.Add(product);

                db.SaveChanges();
                return new upoadNowResult(product.Id, string.Empty, true);
            }
            catch (DbEntityValidationException e)
            {
                string message = string.Empty;
                foreach (var eve in e.EntityValidationErrors)
                    foreach (var ve in eve.ValidationErrors)
                        message += Environment.NewLine + (ve.PropertyName + " " +
                        eve.Entry.CurrentValues.GetValue<object>(ve.PropertyName) + " " +
                        ve.ErrorMessage);
                db.logs.Add(new Log()
                {
                    Location = "upload now api",
                    Message = "" + message
                });
                db.SaveChanges();
                return new upoadNowResult(0, "DbEntityValidationException: " + message, false);
            }
            catch (Exception ex)
            {
                string message = ex.ToString() + ex.InnerException == null ? "" : ex.InnerException.ToString();
                db.logs.Add(new Log()
                {
                    Location = "upload now api",
                    Message = "" + message
                });
                db.SaveChanges();
                return new upoadNowResult(0, message, false);
            }
        }
        private ImageServer getAvaibleServer()
        {
            return db.ImageServers.FirstOrDefault();
        }
        private async Task<ISUploadResult> UploadImage(HttpPostedFileBase model, int ImageServerId)
        {
            ISUploadResult obj = null;
            var iserverid = ImageServerId;
            var iserver = db.ImageServers.Find(iserverid);
            obj = await UploadImageSync(model, "https://" + iserver.Host);
            return obj;
        }
        private async Task<ISUploadResult> UploadImageSync(HttpPostedFileBase model, string uri)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(uri);
            HttpResponseMessage response = await client.PostAsJsonAsync("upload/upload", model);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsAsync<ISUploadResult>();
            return res;
        }
        private async Task<ISResizeViewModel> resize(ResizeViewModel model, int ImageServerId)
        {
            ISResizeViewModel obj = null;
            var iserverid = ImageServerId;
            var iserver = db.ImageServers.Find(iserverid);
            obj = await resizeasync(model, "https://" + iserver.Host);
            return obj;
        }
        private async Task<ISResizeViewModel> resizeasync(ResizeViewModel model, string uri)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(uri);
            HttpResponseMessage response = await client.PostAsJsonAsync("upload/resize", model);
            response.EnsureSuccessStatusCode();
            var res = await response.Content.ReadAsAsync<ISResizeViewModel>();
            return res;
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
