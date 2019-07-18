using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryServices
{
    public class PatronService : IPatron
    {
        private LibraryContext _context;

        public PatronService(LibraryContext context)
        {
            _context = context;
        }

        public void Add(Patron newPatron)
        {
            _context.Add(newPatron);
            _context.SaveChanges();
        }

        public Patron Get(int id)
        {
            return GetAll()
                   .FirstOrDefault(patron => patron.Id == id);
        }

        public IEnumerable<Patron> GetAll()
        {
            return _context.Patrons
                  .Include(patron => patron.LibraryCard)
                  .Include(patron => patron.HomeLibraryBranch);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistories(int patronId)
        {
            var cardId = Get(patronId).LibraryCard.Id;

            return _context.checkoutHistories
                .Include(co => co.LibraryCard)
                .Include(co => co.LibraryAsset)
                .Where(co => co.LibraryCard.Id == cardId)
                .OrderByDescending(co => co.CheckOut);
        }

        public IEnumerable<checkout> GetCheckouts(int patronId)
        {
            var cardId = Get(patronId).LibraryCard.Id;
                
            

            return _context.checkouts
                .Include(co => co.LibraryCard)
                .Include(co => co.LibraryAsset)
                .Where(co => co.LibraryCard.Id == cardId);
        }

        public IEnumerable<Hold> GetHolds(int patronId)
        {
            var cardId = Get(patronId).LibraryCard.Id;

            return _context.Holds
                .Include(h => h.LibraryCard)
                .Include(h => h.LibraryAsset)
                .Where(h => h.LibraryCard.Id == cardId)
                .OrderByDescending(h => h.HoldPlaced);
        }
    }
}
