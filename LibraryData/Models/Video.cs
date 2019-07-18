using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace LibraryData.Models
{
    public class Video: LibraryAsset
    {
        [Required]
        public string Director { get; set; }
    }
}
