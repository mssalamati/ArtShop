using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtShop.Controllers
{
    public class AuctionController : BaseController
    {
        // GET: Auction
        public ActionResult Index(int page = 1)
        {
            int pageSize = 18;
            var p = db.Auctions.Include("Listings").OrderByDescending(x => x.StartTimestamp).AsQueryable();
            var count = p.Count();
            page = Math.Min(page, (int)Math.Ceiling((float)count / (float)pageSize));
            page = Math.Max(1, page);
            ViewBag.page = page;
            ViewBag.count = count;
            ViewBag.pageSize = pageSize;
            p = p.Skip((page - 1) * pageSize).Take(pageSize);
            var res = p.ToList();
            return View(res);
        }
        public ActionResult AuctionArts(int id,int page = 1)
        {
            int pageSize = 18;
            var p = db.Listings.Where(x=>x.auctionInfoId == id).OrderByDescending(x => x.StartTimestamp).AsQueryable();
            var count = p.Count();
            page = Math.Min(page, (int)Math.Ceiling((float)count / (float)pageSize));
            page = Math.Max(1, page);
            ViewBag.page = page;
            ViewBag.count = count;
            ViewBag.pageSize = pageSize;
            p = p.Skip((page - 1) * pageSize).Take(pageSize);
            var res = p.ToList();
            return View(res);
        }
        public ActionResult Art(int id)
        {
            var p = db.Listings.Find(id);
                        
            var artist = p.Artwork.artist_id == "" ? null : db.UserProfiles.FirstOrDefault(a => a.Id == p.Artwork.artist_id);
            ViewBag.Artist = artist;
            if (artist == null)
            {
                ViewBag.artistName = p.Artwork.artistName;
            }
            return View(p);
        }
    }
}