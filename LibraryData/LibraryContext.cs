using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibraryData
{
    public class LibraryContext: DbContext
    {
        public LibraryContext(DbContextOptions options) : base(options) { }
        public DbSet<Patron> Patrons { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<checkout> checkouts { get; set; }
        public DbSet<CheckoutHistory> checkoutHistories { get; set; }
        public DbSet<LibraryBranch> libraryBranches { get; set; }
        public DbSet<BranchHours> BranchHours { get; set; }
        public DbSet<LibraryCard> libraryCards  { get; set; }
        public DbSet<Status> statuses { get; set; }
        public DbSet<LibraryAsset> libraryAssets { get; set; }
        public DbSet<Hold> Holds { get; set; }


    }
}
