using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWebApp.Models.Catalog
{
    public class AssetIndexListingModel
    {
        public int Id { get; set;}
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public string AuthorOrDirectior { get; set; }
        public string Type { get; set; }
        public string DeweyCallNumber { get; set; }
        public string NumberOfCopies { get; set; }
    }
}
