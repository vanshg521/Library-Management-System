using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace LibraryData.Models
{
    public class CheckoutHistory
    {
        public int Id { get; set; }

        [Required]
        public LibraryAsset LibraryAsset { get; set; }

        [Required]
        public LibraryCard LibraryCard { get; set; }

        [Required]
        public DateTime CheckOut { get; set; }

        public DateTime? CheckIn { get; set; }
    }
}
