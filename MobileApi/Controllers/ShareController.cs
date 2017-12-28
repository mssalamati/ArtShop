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
using System.IO;
using Utilities;
using RestSharp;

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
            var material = db.Materials.Where(x => x.AddedByAdmin).Select(x => new
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
            var frames = new
            {
                Colors = db.ProductFrameColors.Select(x => new
                {
                    id = x.Id,
                    name = x.Translations.Any(t => t.languageId == language) ?
                    x.Translations.FirstOrDefault(t => t.languageId == language).Name : string.Empty,
                }),
                Materials = db.ProductFrameMaterials.Select(x => new
                {
                    id = x.Id,
                    name = x.Translations.Any(t => t.languageId == language) ?
                    x.Translations.FirstOrDefault(t => t.languageId == language).Name : string.Empty,
                }),
                Types = db.ProductFrameTypes.Select(x => new
                {
                    id = x.Id,
                    name = x.Translations.Any(t => t.languageId == language) ?
                    x.Translations.FirstOrDefault(t => t.languageId == language).Name : string.Empty,
                }),
            };

            List<object> packages = new List<object>();

            if (language.Contains("en"))
            {
                packages = new List<object>()
                {
                    new { id = (int)Productpackage.box   ,name = "box" } ,
                    new { id = (int)Productpackage.crate ,name = "crate" } ,
                    new { id = (int)Productpackage.tube  ,name = "tube" } ,
                };
            }
            else
            {
                packages = new List<object>()
                {
                    new { id = (int)Productpackage.box   ,name = "جعبه" } ,
                    new { id = (int)Productpackage.crate ,name = "صندوقچه" } ,
                    new { id = (int)Productpackage.tube  ,name = "رول" } ,
                };
            }

            var PaymentsMethods = new List<object>()
            {
                new { id = (int)PaymentMethod.paypall  , name = "paypall" , photoPath = "https://artiscovery.com/Content/Images/PayPal.png"} ,
                new { id = (int)PaymentMethod.zarinpall, name = "zarinpall", photoPath = "https://artiscovery.com/Content/Images/zarrinpal.png" }
            };
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                Pricelists = Pricelists,
                Mediums = Mediums,
                Subjects = Subjects,
                Styles = Styles,
                Categories = Categories,
                material = material,
                frame = frames,
                packages = packages,
                PaymentsMethods = PaymentsMethods,
                userId = User.Identity.GetUserId()
            }, formatter);
        }

        [Route("getMainPages")]
        public HttpResponseMessage getMainPages(string language = "en")
        {
            var result = db.MobileHomePages.Select(x => new
            {
                id = x.Id,
                title = x.Translations.Any(t => t.languageId == language) ?
                x.Translations.FirstOrDefault(t => t.languageId == language).Title : string.Empty,
                photo = x.Items.Take(3).Select(xx => xx.product.Sqphoto.Path)
            });
            return Request.CreateResponse(HttpStatusCode.OK, result, formatter);
        }

        [Route("getMainPageItems")]
        public HttpResponseMessage getMainPageItems(int id, int page = 1)
        {
            var home = db.MobileHomePages.Find(id);
            var products = home.Items.Select(x => x.product).OrderBy(x => x.CreateDate);
            var t = products.Count();
            var result = products.Skip((page - 1) * 10).Take(10).Select(x => new
            {
                id = x.Id,
                title = x.Title,
                photo = x.Widephoto.Path,
                author = x.user.FirstName + " " + x.user.LastName,
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

        [Authorize, Route("getProfileDetail")]
        public HttpResponseMessage getProfileDetail()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            var result = new
            {
                user.Email,
                profile.FirstName,
                profile.LastName,
                profile.Account,
                profile.City,
                profile.countryId,
                profile.isIDConfirmed,
                PhotoPath = profile.PhotoPath,
                profile.profileType,
                profile.Region,
                profile.RegisterDate,
                profile.ZipCode,
                profile.MailingList,
                ArtworksSummery = profile.Products.Take(3).Select(x => new
                {
                    widePhoto = x.Widephoto.Path,
                    squarPhoto = x.Sqphoto.Path
                }),
                ArtworkSize = profile.Products.Count,
                collectionSize = profile.Collections.Count,
                favoritSize = profile.Favorits.Count
            };
            return Request.CreateResponse(HttpStatusCode.OK, result, formatter);
        }

        public class ProfileModel
        {

            public string FirstName { get; set; }
            public string LastName { get; set; }
            public ProfileType UserType { get; set; }
            public bool ReceiveMail { get; set; }
        }

        [Authorize, Route("EditProfile")]
        public HttpResponseMessage EditProfile(ProfileModel obj)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;

            profile.FirstName = obj.FirstName;
            profile.LastName = obj.LastName;

            if (obj.ReceiveMail && !profile.MailingList)
            {
                AddSubscriber(profile.ApplicationUserDetail.Email);
            }
            else if (!obj.ReceiveMail)
            {
                RemoveFromSubscribers(profile.ApplicationUserDetail.Email);
            }

            profile.MailingList = obj.ReceiveMail;
            profile.profileType = obj.UserType;

            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                success = true,
                message = ""
            }, formatter);
        }

        public void AddSubscriber(string email)
        {
            var client = new RestClient("https://api.mailerlite.com/api/v2/groups/7737389/subscribers");
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-mailerlite-apikey", "0e0ba56cc888feb4f4573cfe0a5f497c");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"email\":\"" + email + "\", \"name\": \" \", \"fields\": {\"company\": \"Artiscovery\"}}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }

        public void RemoveFromSubscribers(string email)
        {

            var client = new RestClient("https://api.mailerlite.com/api/v2/groups/7737389/subscribers/" + email);
            var request = new RestRequest(Method.POST);
            request.AddHeader("x-mailerlite-apikey", "0e0ba56cc888feb4f4573cfe0a5f497c");
            //request.AddHeader("content-type", "application/json");
            //request.AddParameter("application/json", "{\"" + email + "\"}", ParameterType.QueryString);
            //request.AddQueryParameter("", email);

            IRestResponse response = client.Delete(request);
        }

        [Route("getProfileById")]
        public HttpResponseMessage getProfileById(string id)
        {
            var user = db.Users.Find(id);
            var profile = user.userDetail;
            var result = new
            {
                profile.FirstName,
                profile.LastName,
                profile.Account,
                profile.City,
                profile.countryId,
                profile.isIDConfirmed,
                PhotoPath = "https://artiscovery.com/" + profile.PhotoPath,
                profile.profileType,
                profile.Region,
                profile.RegisterDate,
                profile.ZipCode,
                ArtworksSummery = profile.Products.Take(3).Select(x => new
                {
                    widePhoto = x.Widephoto.Path,
                    squarPhoto = x.Sqphoto.Path
                }),
                ArtworkSize = profile.Products.Count,
                collectionSize = profile.Collections.Count,
                favoritSize = profile.Favorits.Count
            };
            return Request.CreateResponse(HttpStatusCode.OK, result, formatter);
        }

        public class Avatar
        {
            public string image { get; set; }
        }

        [Authorize, HttpPost, Route("UploadAvatar")]
        public async Task<upoadNowResult> UploadAvatar(Avatar avatar)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;

            var iserver = getAvaibleServer();

            //upload picture
            var UploadResult = await UploadImage(avatar.image, iserver.Id);
            if (!UploadResult.result)
                return new upoadNowResult(0, "Image Server Upload Error: " + UploadResult.data, false);

            profile.PhotoPath = UploadResult.data;
            db.SaveChanges();

            return new upoadNowResult(0, "success", true);
        }

        public class GovId
        {
            public string image { get; set; }
        }

        [Authorize, HttpPost, Route("UploadID")]
        public async Task<upoadNowResult> UploadID(GovId govId)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;

            var iserver = getAvaibleServer();
            //upload picture
            var UploadResult = await UploadImage(govId.image, iserver.Id);
            if (!UploadResult.result)
                return new upoadNowResult(0, "Image Server Upload Error: " + UploadResult.data, false);

            profile.GovermentIdPath = UploadResult.data;
            db.SaveChanges();

            return new upoadNowResult(0, "success", true);
        }

        [HttpGet, Route("FavoritListByProfileId")]
        public HttpResponseMessage FavoritListByProfileId(string id, int page)
        {
            int pageSize = 10;
            var user = db.Users.Find(id);
            var profile = user.userDetail;
            var p = profile.Favorits.AsQueryable();
            var count = p.Count();
            page = Math.Min(page, (int)Math.Ceiling((float)count / (float)pageSize));
            page = Math.Max(1, page);
            p = p.Skip((page - 1) * pageSize).Take(pageSize);
            var result = p.Select(x => new
            {
                Id = x.product.Id,
                Sqphoto = x.product.Sqphoto.Path,
                Title = x.product.Title,
                Description = x.product.Description,
                Photo = x.product.photo.Path,
                WidePhoto = x.product.Widephoto.Path,
                Status = x.product.Status,
                Price = x.product.Price,
            }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                page = page,
                count = count,
                pageSize = pageSize,
                result = result
            }, formatter);
        }

        [Route("getArtworksByProfileId")]
        public HttpResponseMessage getArtworksByProfileId(string id, int page = 1)
        {
            int pageSize = 10;
            var user = db.Users.Find(id);
            var profile = user.userDetail;
            var p = profile.Products.OrderByDescending(a => a.CreateDate).AsQueryable();

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
                AuthorId = x.user_id,
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

        [Authorize, Route("getFavoritList")]
        public HttpResponseMessage getFavoritList(int page)
        {
            int pageSize = 10;
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            var p = profile.Favorits.AsQueryable();
            var count = p.Count();
            page = Math.Min(page, (int)Math.Ceiling((float)count / (float)pageSize));
            page = Math.Max(1, page);
            p = p.Skip((page - 1) * pageSize).Take(pageSize);
            var result = p.Select(x => new
            {
                Id = x.product.Id,
                Sqphoto = x.product.Sqphoto.Path,
                Title = x.product.Title,
                Description = x.product.Description,
                Photo = x.product.photo.Path,
                WidePhoto = x.product.Widephoto.Path,
                Status = x.product.Status,
                Price = x.product.Price,
            }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                page = page,
                count = count,
                pageSize = pageSize,
                result = result
            }, formatter);
        }

        [Authorize, Route("getCollectionList")]
        public HttpResponseMessage getCollectionList()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            var result = profile.Collections.Select(x => new
            {
                Id = x.Id,
                IsPrivate = x.IsPrivate,
                ArtworksSize = x.Artworks.Count,
                Description = x.Description,
                Title = x.Title,
            });
            return Request.CreateResponse(HttpStatusCode.OK, result, formatter);
        }

        [Authorize, Route("getArtworkList")]
        public HttpResponseMessage getArtworkList(int page = 1)
        {
            int pageSize = 10;
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            var p = profile.Products.OrderByDescending(a => a.CreateDate).AsQueryable();

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
                AuthorId = x.user_id,
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

        [Authorize, Route("getOrders")]
        public HttpResponseMessage getOrders()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            var result = db.Orders
               .Include("TransactionDetail")
               .Where(x => x.user_id == userId)
               .OrderByDescending(o => o.BuyDate).Select(x => new
               {
                   orderDetail = x.OrderDetails.Select(a => new
                   {
                       id = a.Product.Id,
                       artworkName = a.Product.Title,
                       artworkPhoto = a.Product.Sqphoto,
                       quantity = a.Quantity,
                       unitPrice = a.UnitPrice
                   }),
                   id = x.Id,
                   Date = x.BuyDate,
                   Status = x.Status,
                   TotalPrice = x.TotalPrice,
                   Payed = x.TransactionDetail.Payed
               });
            return Request.CreateResponse(HttpStatusCode.OK, result, formatter);
        }

        [Authorize, Route("getSales")]
        public HttpResponseMessage getSales()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;
            var result = db.OrderDetails.Include("Product").Include("order")
             .Where(x => x.Product.user_id == userId)
             .Where(x => x.order.TransactionDetail.Payed).Select(a => new
             {
                 a.ProductId,
                 a.Product.Title,
                 a.Product.Sqphoto,
                 a.Quantity,
                 a.UnitPrice,
                 a.type,
                 a.order.ReceiverName,
                 a.order.user_id

             }).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, result, formatter);
        }

        [Authorize, Route("IsIdConfirmed")]
        public HttpResponseMessage IsIdConfirmed()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;

            return Request.CreateResponse(HttpStatusCode.OK, new { isConfirmed = profile.isIDConfirmed }, formatter);
        }

        [Authorize, Route("RemoveArtwork")]
        public HttpResponseMessage RemoveArtwork(int id)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profile = user.userDetail;

            var p = profile.Products.SingleOrDefault(x => x.Id == id);
            if (p != null)
            {
                db.Products.Remove(p);
                db.SaveChanges();
            }

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                success = true,
                message = ""
            }, formatter);
        }

        [Route("getProduct")]
        public HttpResponseMessage getProduct(int id)
        {
            var product = db.Products.Find(id);
            bool isfavorite = false;
            bool isMine = false;
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();

                var currentUserProfile = db.UserProfiles.Find(userId);
                isfavorite = currentUserProfile.Favorits.Any(a => a.productId == id);
                isMine = currentUserProfile.Products.Any(a => a.Id == id);
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { Artwork = product.tojason(), isFavorite = isfavorite, isMine = isMine }, formatter);
        }

        [Authorize, Route("EditProduct")]
        public HttpResponseMessage EditProduct(Product model)
        {
            var p = db.Products.Include("photo").Include("productshippingDetail").Single(x => x.Id == model.Id);
            bool isMine = false;
            var userId = User.Identity.GetUserId();
            var currentUserProfile = db.UserProfiles.Find(userId);            
            isMine = currentUserProfile.Products.Any(a => a.Id == model.Id);
            if (isMine)
            {
                p.Title = model.Title;
                if (currentUserProfile.isIDConfirmed)
                    p.Status = model.Status;
                else
                    p.Status = ProductStatus.NotForSale;
                
                p.TotalWeight = model.TotalWeight;
                p.Height = model.Height;
                p.Width = model.Width;
                p.Depth = model.Depth;
                if (model.Status == ProductStatus.forSale)
                {
                    if (p.user.billingInfo == null)
                        p.user.billingInfo = new BillingInfo();
                    p.user.billingInfo.Street = model.user.billingInfo.Street;
                    p.user.billingInfo.City = model.user.billingInfo.City;
                    if (model.user.billingInfo.CountryId != 0)
                        p.user.billingInfo.CountryId = model.user.billingInfo.CountryId;
                    p.user.billingInfo.PhoneNumber = model.user.billingInfo.PhoneNumber;
                    p.user.billingInfo.Region = model.user.billingInfo.Region;
                    p.user.billingInfo.ZipCode = model.user.billingInfo.ZipCode;
                    
                }

                p.Price = model.Price;
                p.categoryId = model.categoryId;
                p.subjectId = model.subjectId;
                p.ArtCreatedYear = model.ArtCreatedYear;
                p.Mediums.Clear();
                p.Styles.Clear();
                //var medumsList = model.Mediums.Split(',');
                //foreach (var item in medumsList)
                //{
                //    var temp = db.MediumTranslations.FirstOrDefault(x => x.Name == item);
                //    if (temp != null)
                //        p.Mediums.Add(temp.medium);
                //}
                //var styeList = Request["Styleslist"].Split(',');
                //foreach (var item in styeList)
                //{
                //    var temp = db.StyleTranslations.FirstOrDefault(x => x.Name == item);
                //    if (temp != null)
                //        p.Styles.Add(temp.style);
                //}
                p.Materials.Clear();
                foreach (var item in model.MaterialList)
                {
                    var temp = db.Materials.Find(item);
                    if (temp != null)
                        p.Materials.Add(temp);
                }
                p.Description = model.Description;
                p.Keywords = model.Keywords;
                

                db.SaveChanges();
                if (p.user.billingInfo == null)
                    p.user.billingInfo = new BillingInfo();
            }

            return Request.CreateResponse(HttpStatusCode.OK, new { Artwork = p.tojason()}, formatter);
        }

        [HttpGet, Route("Search")]
        public HttpResponseMessage Search(int CategoryId = 0, int StyleId = 0, int SubjectId = 0, int MediumId = 0, int PriceListId = 0, int page = 1, string query = "")
        {
            if (string.IsNullOrEmpty(query))
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
                    AuthorId = x.user_id,
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
            else
            {

                int pageSize = 18;
                var p = db.Products.OrderByDescending(x => x.CreateDate).AsQueryable();
                p = p.Where(x => string.IsNullOrEmpty(query) || x.Title.Contains(query)).AsQueryable();
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


                //if (User.Identity.IsAuthenticated)
                //{
                //    var userId = User.Identity.GetUserId();

                //    var currentUserProfile = db.UserProfiles.Find(userId);
                //    ViewBag.favorites = currentUserProfile.Favorits;
                //}

                return Request.CreateResponse(HttpStatusCode.OK, new
                {
                    page = page,
                    count = count,
                    pageSize = pageSize,
                    result = result
                }, formatter);

            }
        }

        [HttpGet, Route("SearchArtist")]
        public HttpResponseMessage SearchArtist(string query, int page = 1)
        {

            int pageSize = 18;
            var p = db.UserProfiles.OrderByDescending(x => x.LastName).AsQueryable();
            p = p.Where(x => string.IsNullOrEmpty(query) || (x.FirstName + " " + x.LastName).ToLower().Contains(query.ToLower())).AsQueryable();
            var count = p.Count();
            page = Math.Min(page, (int)Math.Ceiling((float)count / (float)pageSize));
            page = Math.Max(1, page);
            p = p.Skip((page - 1) * pageSize).Take(pageSize);
            var result = p.Select(x => new
            {
                Id = x.Id,
                photoPath = x.PhotoPath != "" ? "https://artiscovery.com/" + x.PhotoPath : "",
                firstName = x.FirstName,
                lastName = x.LastName,
                Country = x.country == null ? null : (int?)x.country.Id
            }).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                page = page,
                count = count,
                pageSize = pageSize,
                result = result
            }, formatter);
        }

        [HttpGet, Route("SearchCollection")]
        public HttpResponseMessage SearchCollection(string query, int page = 1)
        {

            int pageSize = 18;
            var p = db.Collections.Include("Artworks").OrderByDescending(x => x.Title).AsQueryable();
            p = p.Where(x => string.IsNullOrEmpty(query) || x.Title.Contains(query)).AsQueryable();
            var count = p.Count();
            page = Math.Min(page, (int)Math.Ceiling((float)count / (float)pageSize));
            page = Math.Max(1, page);
            p = p.Skip((page - 1) * pageSize).Take(pageSize);
            var res = p.Where(a => a.user_id != null).ToList();

            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                page = page,
                count = count,
                pageSize = pageSize,
                result = res
            }, formatter);
        }

        [Authorize, HttpPost, Route("addToFavorite")]
        public HttpResponseMessage addToFavorite(favMV model)
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

        [Authorize, HttpPost, Route("removeFromFavorite")]
        public HttpResponseMessage removeFromFavorite(favMV model)
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

            var fileName = Guid.NewGuid().ToString() + ".png";
            var contentType = "Image/png";
            var res = await uploadnow(model);
            return Request.CreateResponse(HttpStatusCode.OK, new
            {
                success = res.success,
                message = res.error,
                id = res.id
            }, formatter);
        }

        //functions
        public MemoryStream GetSteamFromBase64String(string imageBase64)
        {
            if (imageBase64.IndexOf(',') > 0)
            {
                imageBase64 = imageBase64.Substring(imageBase64.IndexOf(',') + 1);
            }
            byte[] bytes = Convert.FromBase64String(imageBase64);
            var ms = new MemoryStream(bytes);

            return ms;
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
                var UploadResult = await UploadImage(model.imageUpload, iserver.Id);
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
                    Packaging = model.Packaging,
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
                    int id = int.Parse(item);
                    var temp = db.StyleTranslations.FirstOrDefault(x => x.styleId == id);
                    if (temp != null)
                        product.Styles.Add(temp.style);
                }

                foreach (var item in medumsList)
                {
                    int id = int.Parse(item);
                    var temp = db.MediumTranslations.FirstOrDefault(x => x.mediumId == id);
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
        private async Task<ISUploadResult> UploadImage(string model, int ImageServerId)
        {
            ISUploadResult obj = null;
            var iserverid = ImageServerId;
            var iserver = db.ImageServers.Find(iserverid);
            obj = await UploadImageSync(model, "https://" + iserver.Host);
            return obj;
        }
        private async Task<ISUploadResult> UploadImageSync(string model, string uri)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(uri);
            HttpResponseMessage response = await client.PostAsJsonAsync("upload/uploadBase64", new { raw = model });
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
