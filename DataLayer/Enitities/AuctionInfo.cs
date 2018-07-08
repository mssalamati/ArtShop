using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Enitities
{
    public class AuctionInfo
    {
        public int Leader { get; set; }
        public int Outbid { get; set; }
        public int Won { get; set; }
        public int Lost { get; set; }

    }
}