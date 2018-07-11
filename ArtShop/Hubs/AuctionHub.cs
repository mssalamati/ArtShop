using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace ArtShop.Hubs
{
    public class AuctionHub : Hub
    {
        public void Send(string user, string amount, string timestamp, string listingID)
        {
            Clients.All.addBidToPage(user, amount, timestamp, listingID);
        }
    }
}