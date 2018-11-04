using AdminPanel.Models.ViewModel;
using DataLayer.Enitities;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminPanel.Controllers
{
    [Authorize(Roles = "Superadmin,Administrator,Manager")]
    public class AuctionController : BaseController
    {
        public ActionResult AuctionInfo(int page = 1, string search = "")
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;

            var data = db.Auctions
                .Where(x => string.IsNullOrEmpty(search) || x.Translations.Any(t => t.Name.ToLower().Contains(search.ToLower().Trim())));
            count = data.Count();
            data = data.OrderBy(x => x.StartTimestamp).Skip(skip).Take(take);


            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage; ViewBag.search = search;
            return View(data.ToList());
        }
        public ActionResult addAuctionInfo()
        {
            ViewBag.language = db.Languages.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addAuctionInfo(AuctionInfo model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.language = db.Languages.ToList();
                ViewBag.Artworks = db.Products.OrderByDescending(x => x.CreateDate).Take(10);
                return View(model);
            }

            var startTime = model.StartTime.Split(':');
            var endTime = model.EndTime.Split(':');
            DateTime start = new DateTime(model.StartTimestamp.Year, model.StartTimestamp.Month, model.StartTimestamp.Day, int.Parse(startTime[0]), int.Parse(startTime[1]), 0);
            DateTime end = new DateTime(model.EndTimestamp.Year, model.EndTimestamp.Month, model.EndTimestamp.Day, int.Parse(endTime[0]), int.Parse(endTime[1]), 0);

            AuctionInfo newmodel = new AuctionInfo() { Active = model.Active, StartTimestamp = model.StartTimestamp, EndTimestamp = model.EndTimestamp };
            newmodel.Translations = new List<AuctionInfoTranslation>();
            foreach (var item in model.Translations)
                newmodel.Translations.Add(new AuctionInfoTranslation() { languageId = item.languageId, Name = item.Name, Description = item.Description });
            newmodel.Title = newmodel.Translations.FirstOrDefault().Name;
            db.Auctions.Add(newmodel);

            try
            {
                db.SaveChanges();
                return RedirectToAction("AuctionInfo");
            }
            catch (Exception ex)
            {
                ViewBag.language = db.Languages.ToList();
                ModelState.AddModelError(string.Empty, ex.ToString());
                return View(model);
            }
        }

        public ActionResult editAuctionInfo(int id)
        {
            var finder = db.Auctions.Find(id);

            ViewBag.language = db.Languages.ToList();
            AuctionInfo cvm = new AuctionInfo() { Id = finder.Id, StartTimestamp = finder.StartTimestamp, Title = finder.Title, EndTimestamp = finder.EndTimestamp, Active = finder.Active, Translations = new List<AuctionInfoTranslation>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new AuctionInfoTranslation() { languageId = item.languageId, Name = item.Name, Description = item.Description });
            return View(cvm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult editAuctionInfo(AuctionInfo model)
        {
            var finder = db.Auctions.Find(model.Id);

            var startTime = model.StartTime.Split(':');
            var endTime = model.EndTime.Split(':');
            DateTime start = new DateTime(model.StartTimestamp.Year, model.StartTimestamp.Month, model.StartTimestamp.Day, int.Parse(startTime[0]), int.Parse(startTime[1]), 0);
            DateTime end = new DateTime(model.EndTimestamp.Year, model.EndTimestamp.Month, model.EndTimestamp.Day, int.Parse(endTime[0]), int.Parse(endTime[1]), 0);

            finder.StartTimestamp = start;
            finder.EndTimestamp = end;
            finder.Active = model.Active;
            finder.Title = model.Title;


            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                {
                    curr.Name = item.Name;
                    curr.Description = item.Description;
                }
                else
                    finder.Translations.Add(new AuctionInfoTranslation() { languageId = item.languageId, Name = item.Name, Description = item.Description });
            }

            try
            {
                db.SaveChanges();
                return RedirectToAction("AuctionInfo");
            }
            catch (Exception ex)
            {
                ViewBag.Artworks = db.Products.OrderByDescending(x => x.CreateDate).Take(10);
                ModelState.AddModelError(string.Empty, ex.ToString());
            }

            ViewBag.language = db.Languages.ToList();
            AuctionInfo cvm = new AuctionInfo() { Id = finder.Id, StartTimestamp = finder.StartTimestamp, Title = finder.Title, EndTimestamp = finder.EndTimestamp, Active = finder.Active, Translations = new List<AuctionInfoTranslation>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new AuctionInfoTranslation() { languageId = item.languageId, Name = item.Name, Description = item.Description });
            return View(cvm);
        }

        [HttpGet]
        public ActionResult DeleteAuctionInfo(int id)
        {
            var finder = db.Auctions.Find(id);
            finder.Translations.Clear();
            db.Listings.RemoveRange(finder.Listings);
            db.Auctions.Remove(finder);
            db.SaveChanges();
            return RedirectToAction("AuctionInfo");
        }

        // GET: Auction
        public ActionResult Index(int id, int page = 1, string search = "")
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;

            ViewBag.AuctionInfoId = id;
            var data = db.Listings
                .Where(x => string.IsNullOrEmpty(search) || x.Translations.Any(t => t.Name.ToLower().Contains(search.ToLower().Trim()))).Where(a => a.auctionInfoId == id);
            count = data.Count();
            data = data.OrderBy(x => x.StartTimestamp).Skip(skip).Take(take);


            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage; ViewBag.search = search;
            return View(data.ToList());
        }

        public ActionResult addAuction(int id)
        {
            ViewBag.Auctions = db.Auctions.ToList();
            ViewBag.Artworks = db.Products.Where(a=>a.IsAuctionAvailable).OrderByDescending(x => x.CreateDate);
            ViewBag.language = db.Languages.ToList();

            var model = new Listing { auctionInfoId = id };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addAuction(Listing model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Auctions = db.Auctions;
                ViewBag.language = db.Languages.ToList();
                ViewBag.Artworks = db.Products.Where(a => a.IsAuctionAvailable).OrderByDescending(x => x.CreateDate);
                return View(model);
            }

            var startTime = model.StartTime.Split(':');
            var endTime = model.EndTime.Split(':');


            DateTime start = new DateTime(model.StartTimestamp.Year, model.StartTimestamp.Month, model.StartTimestamp.Day, int.Parse(startTime[0]), int.Parse(startTime[1]), 0);
            DateTime end = new DateTime(model.EndTimestamp.Year, model.EndTimestamp.Month, model.EndTimestamp.Day, int.Parse(endTime[0]), int.Parse(endTime[1]), 0);

            Listing newmodel = new Listing() {Title = model.Title, Active = model.Active, StartTimestamp = start, ShowWinner = model.ShowWinner, StartingPrice = model.StartingPrice, EndTimestamp = end, BidStep = model.BidStep, auctionInfoId = model.auctionInfoId };
            newmodel.Translations = new List<ListingTranslation>();
            foreach (var item in model.Translations)
                newmodel.Translations.Add(new ListingTranslation() { languageId = item.languageId, Name = item.Name, Description = item.Description });
            newmodel.ArtworkId = model.ArtworkId;
            newmodel.Title = newmodel.Translations.FirstOrDefault().Name;
            db.Listings.Add(newmodel);

            try
            {
                db.SaveChanges();
                return RedirectToAction("Index", new { id = model.auctionInfoId });
            }
            catch (Exception ex)
            {
                ViewBag.language = db.Languages.ToList();
                ViewBag.Artworks = db.Products.OrderByDescending(x => x.CreateDate).Take(10);
                ModelState.AddModelError(string.Empty, ex.ToString());
                return View(model);
            }
        }
        public ActionResult editAuction(int id)
        {
            var finder = db.Listings.Find(id);
            ViewBag.Auctions = db.Auctions;
            ViewBag.Artworks = db.Products.Where(a => a.IsAuctionAvailable).OrderByDescending(x => x.CreateDate);
            ViewBag.language = db.Languages.ToList();
            Listing cvm = new Listing() { ListingID = finder.ListingID, StartingPrice = finder.StartingPrice, StartTimestamp = finder.StartTimestamp, EndTimestamp = finder.EndTimestamp, BidStep = finder.BidStep, ArtworkId = finder.ArtworkId, auctionInfoId = finder.auctionInfoId, Title = finder.Title, Active = finder.Active, ShowWinner = finder.ShowWinner, Translations = new List<ListingTranslation>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new ListingTranslation() { languageId = item.languageId, Name = item.Name, Description = item.Description });
            return View(cvm);
        }

        [HttpPost]
        public ActionResult editAuction(Listing model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Auctions = db.Auctions;
                ViewBag.language = db.Languages.ToList();
                ViewBag.Artworks = db.Products.Where(a => a.IsAuctionAvailable).OrderByDescending(x => x.CreateDate);
                return View(model);
            }

            var finder = db.Listings.Find(model.ListingID);

            var startTime = model.StartTime.Split(':');
            var endTime = model.EndTime.Split(':');
            DateTime start = new DateTime(model.StartTimestamp.Year, model.StartTimestamp.Month, model.StartTimestamp.Day, int.Parse(startTime[0]), int.Parse(startTime[1]), 0);
            DateTime end = new DateTime(model.EndTimestamp.Year, model.EndTimestamp.Month, model.EndTimestamp.Day, int.Parse(endTime[0]), int.Parse(endTime[1]), 0);

            finder.StartTimestamp = start;
            finder.EndTimestamp = end;
            finder.Active = model.Active;
            finder.Title = model.Title;
            finder.ShowWinner = model.ShowWinner;
            finder.ArtworkId = model.ArtworkId;

            foreach (var item in model.Translations)
            {
                var curr = finder.Translations.SingleOrDefault(x => x.languageId == item.languageId);
                if (curr != null)
                {
                    curr.Name = item.Name;
                    curr.Description = item.Description;
                }
                else
                    finder.Translations.Add(new ListingTranslation() { languageId = item.languageId, Name = item.Name, Description = item.Description });
            }

            try
            {
                db.SaveChanges();
                return RedirectToAction("Index", new { id = model.auctionInfoId });
            }
            catch (Exception ex)
            {
                ViewBag.Artworks = db.Products.Where(a => a.IsAuctionAvailable).OrderByDescending(x => x.CreateDate);
                ModelState.AddModelError(string.Empty, ex.ToString());
            }

            ViewBag.language = db.Languages.ToList();
            Listing cvm = new Listing() { ListingID = finder.ListingID, Active = finder.Active, ShowWinner = finder.ShowWinner, auctionInfoId = finder.auctionInfoId, StartingPrice = finder.StartingPrice, EndTimestamp = finder.EndTimestamp, ArtworkId = finder.ArtworkId, Title = finder.Title, Translations = new List<ListingTranslation>() };
            foreach (var item in finder.Translations)
                cvm.Translations.Add(new ListingTranslation() { languageId = item.languageId, Name = item.Name, Description = item.Description });
            return View(cvm);
        }
        [HttpGet]
        public ActionResult DeleteAuction(int id)
        {
            var finder = db.Listings.Find(id);
            finder.Translations.Clear();
            db.Bids.RemoveRange(finder.Bids);
            db.Listings.Remove(finder);

            db.SaveChanges();
            return RedirectToAction("Index", new { id = finder.auctionInfoId });
        }

        public ActionResult Bids(int id, int page = 1, string search = "")
        {
            int count = 0, pagesize = 15, take = pagesize, skip = (page - 1) * pagesize;
            ViewBag.Id = id;
            var data = db.Bids
                .Where(x => string.IsNullOrEmpty(search) || x.User.ApplicationUserDetail.Email.ToLower().Contains(search.ToLower().Trim())).Where(a => a.ListingID == id);
            count = data.Count();
            data = data.OrderBy(x => x.CurrentPrice).Skip(skip).Take(take);

            int maxpage = count % pagesize != 0 ? (count / pagesize) + 1 : (count / pagesize);
            ViewBag.page = page; ViewBag.maxpage = maxpage; ViewBag.search = search;
            return View(data.ToList());
        }

        public ActionResult ArtworkPartial()
        {
            var products = db.Products.OrderByDescending(x => x.CreateDate);
            return PartialView(products);
        }
    }
}