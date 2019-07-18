using LibraryData.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryData
{
    public interface IPatron
    {
        Patron Get(int id);
        IEnumerable<Patron> GetAll();
        void Add(Patron newPatron);

        IEnumerable<CheckoutHistory> GetCheckoutHistories(int patronId);
        IEnumerable<Hold> GetHolds(int patronId);
        IEnumerable<checkout> GetCheckouts(int patronId);


    }
}
