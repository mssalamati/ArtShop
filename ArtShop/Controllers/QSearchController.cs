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
        public ActionResult Artist(string query, int page = 1)
        {
            int pageSize = 18;
            var p = db.UserProfiles.OrderByDescending(x => x.LastName).AsQueryable();
            p = p.Where(x => string.IsNullOrEmpty(query) || x.LastName.Contains(query) || x.LastName.Contains(query)).AsQueryable();
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
        public ActionResult Collection(string query, int page = 1)
        {
            int pageSize = 18;
            var p = db.Collections.OrderByDescending(x => x.Title).AsQueryable();
            p = p.Where(x => string.IsNullOrEmpty(query) || x.Title.Contains(query)).AsQueryable();
            var count = p.Count();
            page = Math.Min(page, (int)Math.Ceiling((float)count / (float)pageSize));
            page = Math.Max(1, page);
            ViewBag.page = page;
            ViewBag.query = query;
            ViewBag.count = count;
            ViewBag.pageSize = pageSize;
            p = p.Skip((page - 1) * pageSize).Take(pageSize);
            var res = p.ToList();
            return View(res);
        }
    }
}