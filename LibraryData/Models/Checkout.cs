using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace LibraryData.Models
{
    public class checkout
    {
        public int Id { get; set; }

        [Required]
        public LibraryAsset LibraryAsset { get; set; }
        public LibraryCard LibraryCard { get; set; }
        public DateTime Since { get; set; }
        public DateTime Until { get; set; }

    }
}
