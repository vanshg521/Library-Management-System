using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData
{
    public interface ICheckout
    {
       
        checkout GetById(int checkoutId);
        checkout GetLastestCheckout(int id);

        void Add(checkout newCheckout);
        void CheckOutItem(int assetId, int libraryCardId);
        void CheckInItem(int assetId);
        void MarkLost(int assetId);
        void MarkFound(int assetId);

        IEnumerable<CheckoutHistory> GetCheckoutHistory(int id);
        IEnumerable<checkout> GetAll();
        IEnumerable<Hold> GetCurrentHolds(int id);

        string GetCurrentCheckoutPatron(int assetId);
        string GetCurrentHoldPatronName(int id);
        string GetCurrentPatron(int id);
        int GetNumberOfCopies(int id);
        bool IsCheckedOut(int id);
        void PlaceHold(int assetId, int libraryCardId);
       
        DateTime GetCurrentHoldPlaced(int id);
       

        


    }
}
