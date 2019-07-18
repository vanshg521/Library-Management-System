using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWebApp.Models.Catalog
{
    public class AssetDetailModel
    {
        public int AssetId { get; set; }
        public string Title { get; set; }
        public string AuthorOrDirector { get; set; }
        public string Type { get; set;}
        public int Year { get; set; }
        public string ISBN { get; set; }
        public string DeweyCallNumber { get; set; }
        public string Status { get; set; }
        public decimal Cost { get; set; }
        public string CurrentLocation { get; set; }
        public string ImageUrl { get; set; }
        public string PatronName { get; set; }
        public checkout LastestCheckout  { get; set; }
        public IEnumerable<CheckoutHistory> CheckoutHistory { get; set; }
        public IEnumerable<AssetHoldModel> CurrentHolds { get; set; }


    }
    public class AssetHoldModel
    {
        public string PatronName { get; set; }
        public DateTime HoldPlaced { get; set; }


    }
}
