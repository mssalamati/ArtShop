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

        [Route("getMainPages")]
        public HttpResponseMessage getMainPages()
        {
            var thirdpictest = db.Products.Take(3).Select(x => x.Sqphoto.Path).ToList();
            var fakedata = new List<object>()
            {
                new { id = 1, title = "انتخاب 1", photo = thirdpictest },
                new { id = 2, title = "انتخاب 2", photo = thirdpictest },
                new { id = 3, title = "انتخاب 3", photo = thirdpictest },
                new { id = 4, title = "انتخاب 4", photo = thirdpictest },
                new { id = 5, title = "انتخاب 5", photo = thirdpictest },
                new { id = 6, title = "انتخاب 6", photo = thirdpictest },
                new { id = 7, title = "انتخاب 7", photo = thirdpictest },
                new { id = 8, title = "انتخاب 8", photo = thirdpictest },
                new { id = 9, title = "انتخاب 9", photo = thirdpictest },
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

        [Route("getCounties")]
        public HttpResponseMessage getCounties(string language)
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
