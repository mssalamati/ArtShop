using ArtShop.Helper;
using DataLayer.Enitities;
using Postal;
using reCaptcha;
using Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ArtShop.Controllers
{
    public class SupportController : BaseController
    {
        // GET: Support
        public ActionResult Index()
        {
            var data = db.Articles.Where(a => a.isHandbook == true).ToList();
            return View(data);
        }

        public ActionResult Search(string keyword)
        {
            string currentCultureName = CultureInfo.CurrentCulture.Name.Substring(0, 2);
            ViewBag.keyword = keyword;
            var result = db.Articles.Where(a => a.Translations.FirstOrDefault(x => x.languageId == currentCultureName).Title.Contains(keyword)).ToList();
            return View(result);
        }

        public ActionResult Article(int id)
        {
            var article = db.Articles.Find(id);
            return View(article);
        }

        public ActionResult Category(int id)
        {
            var category = db.SupportCategories.Find(id);
            return View(category);
        }

        public ActionResult SubCategory(int id)
        {
            var subCategory = db.SupportSubCategories.Find(id);
            return View(subCategory);
        }

        public ActionResult Header()
        {

            return PartialView("_Header");
        }

        public ActionResult SetCulture(string culture)
        {
            HttpCookie popupCookie = Request.Cookies["isShown"];
            if (popupCookie != null)
                popupCookie.Value = "true";   // update cookie value
            else
            {
                popupCookie = new HttpCookie("isShown");
                popupCookie.Value = "true";
                popupCookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(popupCookie);


            culture = CultureHelper.GetImplementedCulture(culture);
            HttpCookie cookie = Request.Cookies["_culture"];
            if (cookie != null)
                cookie.Value = culture;   // update cookie value
            else
            {
                cookie = new HttpCookie("_culture");
                cookie.Value = culture;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            string url = this.Request.UrlReferrer.AbsolutePath + this.Request.UrlReferrer.Query ?? "";
            return Redirect(url);
        }

        public ActionResult requests(int type = 0)
        {
            ViewBag.Recaptcha = ReCaptcha.GetHtml(ConfigurationManager.AppSettings["ReCaptcha:SiteKey"]);
            ViewBag.publicKey = ConfigurationManager.AppSettings["ReCaptcha:SiteKey"];
            if (type == 0)
            {
                var questions = ShareRes.FAQ_Artist_questions.Split(',').Select(x => new { name = x });
                ViewBag.question = new SelectList(questions, "name", "name");
            }
            else if (type == 1)
            {
                var questions = ShareRes.FAQ_Collector_questions.Split(',').Select(x => new { name = x });
                ViewBag.question = new SelectList(questions, "name", "name");
            }
            return View(new FAQRequest() { type = type });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult requests(FAQRequest model)
        {      
            if (ModelState.IsValid && ReCaptcha.Validate(ConfigurationManager.AppSettings["ReCaptcha:SecretKey"]))
            {
                SendTicket(model);
                return RedirectToActionPermanent("index");
            }
            ViewBag.RecaptchaLastErrors = ReCaptcha.GetLastErrors(this.HttpContext);
            ViewBag.publicKey = ConfigurationManager.AppSettings["ReCaptcha:SiteKey"];
            if (model.type == 0)
            {
                var questions = ShareRes.FAQ_Artist_questions.Split(',').Select(x => new { name = x });
                ViewBag.question = new SelectList(questions, "name", "name");
            }
            else if (model.type == 1)
            {
                var questions = ShareRes.FAQ_Collector_questions.Split(',').Select(x => new { name = x });
                ViewBag.question = new SelectList(questions, "name", "name");
            }
            return View(model);
        }
        private void SendTicket(FAQRequest model)
        {
            var artistQuestions = ShareRes.FAQ_Artist_questions.Split(',').Select(x => new { name = x });
            var collectorQuestions = ShareRes.FAQ_Collector_questions.Split(',').Select(x => new { name = x });

            dynamic email = new Email("Ticket");
            email.To = "support@artiscovery.freshdesk.com";
            email.Subject = model.subject;
            
            string type = "";
            switch (model.type)
            {
                case 0:
                    type = ShareRes.FAQ_Type_0;
                    break;
                case 1:
                    type = ShareRes.FAQ_Type_1;
                    break;
                case 2:
                    type = ShareRes.FAQ_Type_2;
                    break;
                case 3:
                    type = ShareRes.FAQ_Type_3;
                    break;
                default:
                    break;
            }

            email.email = model.email;
            email.subject = model.subject;
            email.question = model.question;
            email.name = model.Name;
            email.profileURL = model.URL;
            email.artistName = model.ArtistName;
            email.artworkTitle = model.ArtworkTitle;
            email.phoneNumber = model.PhoneNumber;
            email.detail = model.description;
            email.requestType = type;
            email.type = model.type;
            email.Attach(new Attachment(model.Attachments.InputStream,model.Attachments.FileName));
            email.Send();
        }

    }
}