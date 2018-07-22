using ArtShop.Hubs;
using DataLayer.Enitities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
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
        public ActionResult AuctionArts(int id, int page = 1)
        {
            int pageSize = 18;
            var p = db.Listings.Where(x => x.auctionInfoId == id).OrderByDescending(x => x.StartTimestamp).AsQueryable();
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

        [System.Web.Mvc.Authorize]
        public ActionResult Art(int id)
        {
            var p = db.Listings.Find(id);

            var artist = p.Artwork.artist_id == "" ? null : db.UserProfiles.FirstOrDefault(a => a.Id == p.Artwork.artist_id);
            ViewBag.Artist = artist;
            if (artist == null)
            {
                ViewBag.artistName = p.Artwork.artistName;
            }

            Bid latestBid = null;
            foreach (var b in p.Bids.OrderByDescending(l => l.Timestamp).Take(1))
                latestBid = b;

            if (latestBid == null)
            {
                ViewBag.CurrentPrice = p.StartingPrice;
            }
            else
            {
                ViewBag.CurrentPrice = latestBid.Amount;
            }

            Session["CurrentListingID"] = id;

            return View(p);
        }
        [System.Web.Mvc.Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Art(Bid bid)
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            var profileId = user.userDetail.Id;

            int currentListingID = Convert.ToInt32(Session["CurrentListingID"]);
            Listing currentListingOwner = db.Listings.FirstOrDefault(l => l.ListingID == currentListingID);
            Listing listing = db.Listings.Find(currentListingID);
            Bid latestBid = null;
            foreach (var b in listing.Bids.OrderByDescending(l => l.Timestamp).Take(1))
                latestBid = b;
            if (latestBid == null)
            {
                ViewBag.CurrentPrice = listing.StartingPrice;
                ViewBag.NextPrice = listing.StartingPrice + listing.BidStep;
            }
            else
            {
                ViewBag.CurrentPrice = latestBid.Amount;
                ViewBag.NextPrice = latestBid.Amount;
            }
            // Check if the link is valid
            if (ModelState.IsValid)
            {
                // Check for consecutive bids

                // Dirty workaround

                // Can't bid on finished auctions                   
                if (listing.EndTimestamp.Subtract(DateTime.Now).Seconds > 0)
                {
                    // Can't bid on the same auction twice
                    if (latestBid == null || latestBid.UserID != profileId)
                    {
                        // Amount must be greater than the last bid and starting big
                        if ((latestBid == null && bid.Amount >= listing.StartingPrice) ||
                            (latestBid != null && bid.Amount > latestBid.Amount && (bid.Amount - latestBid.Amount == listing.BidStep)))
                        {

                            if (latestBid == null)
                            {
                                ViewBag.CurrentPrice = listing.StartingPrice;
                                ViewBag.NextPrice = listing.StartingPrice + listing.BidStep;
                            }
                            else
                            {
                                ViewBag.CurrentPrice = bid.Amount;
                                ViewBag.NextPrice = latestBid.Amount + listing.BidStep;
                            }

                            bid.UserID = profileId;
                            bid.User = db.UserProfiles.FirstOrDefault(u => u.Id == bid.UserID);
                            bid.ListingID = Convert.ToInt32(Session["CurrentListingID"]);
                            bid.Listing = db.Listings.FirstOrDefault(l => l.ListingID == bid.ListingID);
                            bid.Timestamp = DateTime.Now;

                            db.Bids.Add(bid);
                            db.SaveChanges();

                            DefaultHubManager hd = new DefaultHubManager(GlobalHost.DependencyResolver);
                            var context = GlobalHost.ConnectionManager.GetHubContext<AuctionHub>();
                            context.Clients.All.addBidToPage(bid.User.ApplicationUserDetail.UserName, bid.Amount.ToString(), bid.Timestamp.ToString(), bid.ListingID.ToString());
                            return View(listing);
                        }
                        else
                        {
                            Error(5);
                            return View(listing);
                        }
                    }
                    else
                    {
                        Error(4);
                        return View(listing);
                    }
                }
                else
                {
                    Error(3);
                    return View(listing);
                }

            }
            Error(1);
            return View(listing);

        }

        public void Error(int ErrorID)
        {
            switch (ErrorID)
            {
                case 1:
                    ViewBag.ErrorText = "Invalid input. Please try again.";
                    break;
                case 2:
                    ViewBag.ErrorText = "You cannot bid on your own listing.";
                    break;
                case 3:
                    ViewBag.ErrorText = "This auction has already ended.";
                    break;
                case 4:
                    ViewBag.ErrorText = "You cannot bid twice in a row.";
                    break;
                case 5:
                    ViewBag.ErrorText = "Your bid amount must be higher than the previous bid or starting price.";
                    break;
                default:
                    ViewBag.ErrorText = "Something went wrong.";
                    break;
            }
        }
    }
}