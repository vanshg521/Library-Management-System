using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace LibraryData.Models
{
    public class LibraryCard
    {
        public int Id { get; set; }

        public decimal Fees { get; set; }

        public DateTime Created { get; set; }

        public virtual IEnumerable<checkout> Checkouts { get; set; }
    }
}
