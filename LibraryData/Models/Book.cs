using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace LibraryData.Models
{
    public class Book : LibraryAsset
    {
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        public string DeweyIndex { get; set; }
    }
}
