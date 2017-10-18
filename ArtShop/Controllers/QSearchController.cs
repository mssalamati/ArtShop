using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArtShop.Controllers
{
    public class QSearchController : BaseController
    {
        [Route("qsearch/artist/{query}/{page?}")]
        [Route("qsearch/artist")]
        public ActionResult Artist(string query, int page = 1)
        {
            int pageSize = 18;
            var p = db.UserProfiles.OrderByDescending(x => x.LastName).AsQueryable();
            p = p.Where(x => string.IsNullOrEmpty(query) || (x.FirstName + " "+ x.LastName).ToLower().Contains(query.ToLower())).AsQueryable();
            var count = p.Count();
            page = Math.Min(page, (int)Math.Ceiling((float)count / (float)pageSize));
            page = Math.Max(1, page);
            ViewBag.query = query;
            ViewBag.page = page;
            ViewBag.count = count;
            ViewBag.pageSize = pageSize;
            p = p.Skip((page - 1) * pageSize).Take(pageSize);
            var res = p.ToList();
            return View(res);
        }

        [Route("qsearch/art/{query}/{page?}")]
        [Route("qsearch/art")]
        public ActionResult Art(string query, int page = 1)
        {
            int pageSize = 18;
            var p = db.Products.OrderByDescending(x => x.CreateDate).AsQueryable();
            p = p.Where(x => string.IsNullOrEmpty(query) || x.Title.Contains(query)).AsQueryable();
            var count = p.Count();
            page = Math.Min(page, (int)Math.Ceiling((float)count / (float)pageSize));
            page = Math.Max(1, page);
            ViewBag.query = query;
            ViewBag.page = page;
            ViewBag.count = count;
            ViewBag.pageSize = pageSize;
            p = p.Skip((page - 1) * pageSize).Take(pageSize);
            var res = p.ToList();
            return View(res);
        }

        [Route("qsearch/collection/{query}/{page?}")]
        [Route("qsearch/collection")]
        public ActionResult Collection(string query, int page = 1)
        {
            int pageSize = 18;
            var p = db.Collections.Include("Artworks").OrderByDescending(x => x.Title).AsQueryable();
            p = p.Where(x => string.IsNullOrEmpty(query) || x.Title.Contains(query)).AsQueryable();
            var count = p.Count();
            page = Math.Min(page, (int)Math.Ceiling((float)count / (float)pageSize));
            page = Math.Max(1, page);
            ViewBag.page = page;
            ViewBag.query = query;
            ViewBag.count = count;
            ViewBag.pageSize = pageSize;
            p = p.Skip((page - 1) * pageSize).Take(pageSize);
            var res = p.Where(a=>a.user_id != null).ToList();
            return View(res);
        }
    }
}