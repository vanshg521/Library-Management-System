using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryServices
{
    public class CheckoutService : ICheckout
    {
        private LibraryContext _context;

        public CheckoutService(LibraryContext context) {
            _context = context;

        }
        public void Add(checkout newCheckout)
        {
            _context.Add(newCheckout);
            _context.SaveChanges();
        }

        public void CheckInItem(int assetId)
        {
            var now = DateTime.Now;

            var item = _context.libraryAssets.FirstOrDefault(a => a.Id == assetId);


            //remove any existing checkouts on the item
            RemoveExistingCheckouts(assetId);
            //close any existing holds on the item
            CloseExistingCheckoutHistory(assetId, now);
            //look for existing holds on the item
            var currentHolds = _context.Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .Where(h => h.LibraryAsset.Id == assetId);
            //if there are holds, checkout the item to the librarycard with earliest hold
            //otherwise, update the item status to available
            if (currentHolds.Any()) {
                CheckoutToEarliestHold(assetId, currentHolds);
                return;
            }
            UpdateAssetStatus(assetId, "Available");
            _context.SaveChanges();
        }

        private void CheckoutToEarliestHold(int assetId, IQueryable<Hold> currentHolds)
        {
            var earliestHold = currentHolds
                .OrderBy(holds => holds.HoldPlaced)
                .FirstOrDefault();

            var card = earliestHold.LibraryCard;

            _context.Remove(earliestHold);
            _context.SaveChanges();
            CheckInItem(assetId);
        }
      
        public void CheckOutItem(int assetId, int libraryCardId)
        {
            if (IsCheckedOut(assetId)) {
                return;
            }
            var item = _context.libraryAssets
                .FirstOrDefault(a => a.Id == assetId);


            UpdateAssetStatus(assetId, "Checked out");
            var LibraryCard = _context.libraryCards
                .Include(card => card.Checkouts)
                .FirstOrDefault(card => card.Id == libraryCardId);
            var now = DateTime.Now;

            var checkout = new checkout
            {
                LibraryAsset = item,
                LibraryCard = LibraryCard,
                Since = now,
                Until = GetDefaultCheckoutTime(now)
            };
            _context.Add(checkout);

            var checkoutHistory = new CheckoutHistory
            {
                CheckOut = now,
                LibraryAsset = item,
                LibraryCard = LibraryCard
            };
            _context.Add(checkoutHistory);

            _context.SaveChanges();
        }

        private DateTime GetDefaultCheckoutTime(DateTime now)
        {
            return now.AddDays(30);
        }

        public bool IsCheckedOut(int assetId)
        {
            return _context.checkouts
                  .Where(co => co.LibraryAsset.Id == assetId)
                  .Any();
            
        }

        public IEnumerable<checkout> GetAll()
        {
           return _context.checkouts; 
        }

        public checkout GetById(int checkoutId)
        {
            return GetAll()
                .FirstOrDefault(checkout => checkout.Id == checkoutId);
        }

        public IEnumerable<CheckoutHistory> GetCheckoutHistory(int id)
        {
            return _context.checkoutHistories
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .Where(h => h.LibraryAsset.Id == id);
        }

      

        public IEnumerable<Hold> GetCurrentHolds(int id)
        {
            return _context.Holds
                 .Include(h => h.LibraryAsset)
                 .Where(h => h.LibraryAsset.Id == id);
        }

        public checkout GetLastestCheckout(int assetId) {
            return _context.checkouts.Where(c => c.LibraryAsset.Id == assetId)
            .OrderByDescending(c => c.Since)
            .FirstOrDefault();

        }

        public void MarkFound(int assetId)
        {
            var now = DateTime.Now;
            UpdateAssetStatus(assetId, "Available");
            RemoveExistingCheckouts(assetId);

            CloseExistingCheckoutHistory(assetId, now);
          
             _context.SaveChanges();
        }

        private void UpdateAssetStatus(int assetId, string newStatus)
        {
            
            var item = _context.libraryAssets
               .FirstOrDefault(a => a.Id == assetId);

            _context.Update(item);

            item.Status = _context.statuses
                .FirstOrDefault(status => status.Name == newStatus);
        }

        private void CloseExistingCheckoutHistory(int assetId, DateTime now)
        {
            //close any existing checkout history
            var history = _context.checkoutHistories.FirstOrDefault(h => h.LibraryAsset.Id == assetId
            && h.CheckIn == null);

            if (history != null)
            {
                _context.Update(history);
                history.CheckIn = now;
            }
        }

        private void RemoveExistingCheckouts(int assetId)
        {
            //remove any existing checkouts on the item
            var checkout = _context.checkouts
                .FirstOrDefault(co => co.LibraryAsset.Id == assetId);

            if (checkout != null)
            {
                _context.Remove(checkout);
            }
        }

        public void MarkLost(int assetId)
        {
            UpdateAssetStatus(assetId, "Lost");

            _context.SaveChanges();
        }

        public void PlaceHold(int assetId, int libraryCardId)
        {
            var now = DateTime.Now;

            var asset = _context.libraryAssets
                .Include(a=> a.Status)
                .FirstOrDefault(a => a.Id == assetId);

            var card = _context.libraryCards
                .FirstOrDefault(c => c.Id == libraryCardId);

            if (asset.Status.Name == "Available") {
                UpdateAssetStatus(assetId, "On Hold");
            }

            var hold = new Hold
            {
                HoldPlaced = now,
                LibraryAsset = asset,
                LibraryCard = card
            };

            _context.Add(hold);
            _context.SaveChanges();
           
        }
        public string GetCurrentHoldPatronName(int holdId)
        {
            var hold = _context.Holds
                .Include(h => h.LibraryAsset)
                .Include(h => h.LibraryCard)
                .FirstOrDefault(h => h.Id == holdId);

            var cardId = hold?.LibraryCard.Id;
            var patron = _context.Patrons.Include(p => p.LibraryCard)
                .FirstOrDefault(p => p.LibraryCard.Id == cardId);

            return patron?.FirstName + "" + patron?.LastName;
        }

        public DateTime GetCurrentHoldPlaced(int holdId)
        {
            return
                 _context.Holds
                 .Include(h => h.LibraryAsset)
                 .Include(h => h.LibraryCard)
                 .FirstOrDefault(h => h.Id == holdId)
                 .HoldPlaced;
        }


        public int GetNumberOfCopies(int id)
        {
            throw new NotImplementedException();
        }

       

        public string GetCurrentPatron(int id)
        {
            throw new NotImplementedException();
        }

        public string GetCurrentCheckoutPatron(int assetId)
        {
            var checkout = GetCheckoutByAssetId(assetId);
            if (checkout == null) {
                return "Not Checked out";
            }

            var cardId = checkout.LibraryCard.Id;

            var Patron = _context.Patrons
                .Include(P => P.LibraryCard)
                .FirstOrDefault(p => p.LibraryCard.Id == cardId);

            return Patron.FirstName + "" + Patron.LastName;
        }

        private checkout GetCheckoutByAssetId(int assetId)
        {
           return  _context.checkouts
                 .Include(co => co.LibraryAsset)
                 .Include(co => co.LibraryCard)
                 .Where(co => co.LibraryAsset.Id == assetId)
                 .FirstOrDefault(co => co.LibraryAsset.Id == assetId);
        }

       
    }
}
