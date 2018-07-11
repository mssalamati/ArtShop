using ArtShop.Controllers;
using ArtShop.Util;
using DataLayer;
using DataLayer.Enitities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ArtShop
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static CacheItemRemovedCallback OnCacheRemove = null;
        ApplicationDbContext db = new ApplicationDbContext();
        HttpContext context;
        protected void Application_Start()
        {
            context = HttpContext.Current;

            AddTask("RefreshBids", 10);

            if (context != null && context.Session != null)
            {
                Session["CurrentUsername"] = "";
                Session["CurrentUserID"] = 0;
            }

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //GlobalFilters.Filters.Add(new RequireHttpsAttribute());
        }
        private void AddTask(string name, int seconds)
        {
            OnCacheRemove = new CacheItemRemovedCallback(CacheItemRemoved);
            HttpRuntime.Cache.Insert(name, seconds, null,
                DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable, OnCacheRemove);
        }

        public void CacheItemRemoved(string k, object v, CacheItemRemovedReason r)
        {
            if (k.Equals("RefreshBids"))
            {
                // do stuff here if it matches our taskname, like WebRequest
                // re-add our task so it recurs 
                var finishedListings = db.Listings.Where(l => DbFunctions.DiffSeconds(l.EndTimestamp, DateTime.Now) > 0);
                foreach (var listing in finishedListings)
                {
                    // If there are no closing histories found for the listing
                    // Only create closing histories for "won" auctions
                    if (!db.ClosingHistories.Any(ch => ch.ListingID == listing.ListingID) && listing.Bids.Count > 0)
                    {
                        // Get the winning bid (dirty workaround)
                        Bid winningBid = null;
                        foreach (var b in listing.Bids.OrderByDescending(l => l.Timestamp).Take(1))
                            winningBid = b;

                        // Save data to closing history
                        ClosingHistory ch = new ClosingHistory();
                        ch.BidID = winningBid.BidID;
                        ch.Bid = winningBid;
                        ch.ListingID = listing.ListingID;
                        ch.Listing = listing;
                        ch.UserID = winningBid.UserID;
                        ch.User = winningBid.User;

                        // Save closing history to db
                        db.ClosingHistories.Add(ch);

                    }
                }

                db.SaveChanges();
            }

            AddTask(k, Convert.ToInt32(v));
        }
    }
}
