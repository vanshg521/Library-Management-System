using LibraryData;
using LibraryData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibraryServices
{
    public class LibraryBranchService : ILibraryBranch
    {
        private LibraryContext _context;

        public LibraryBranchService(LibraryContext context)
        {
            _context = context;
        }

        public void Add(LibraryBranch newBranch)
        {
            _context.Add(newBranch);
            _context.SaveChanges();
        }

        public LibraryBranch Get(int branchId)
        {
            return GetAll()
                 .FirstOrDefault(b => b.Id == branchId);
        }

        public IEnumerable<LibraryBranch> GetAll()
        {
            return _context.libraryBranches
                   .Include(b => b.Patrons)
                   .Include(b => b.LibraryAssets);
        }

        public IEnumerable<LibraryAsset> GetAssets(int branchId)
        {
            return _context.libraryBranches
                .Include(b => b.LibraryAssets)
                .FirstOrDefault(b => b.Id == branchId)
                .LibraryAssets;

            
        }

        public IEnumerable<string> GetBranchHours(int branchId)
        {
            var hours = _context.BranchHours.Where(h => h.Branch.Id == branchId);

            return DataHelpers.HumanizeBizHours(hours);

        }

        public IEnumerable<Patron> GetPatrons(int branchId)
        {
            return _context.libraryBranches
                .Include(b => b.Patrons)
                .FirstOrDefault(b => b.Id == branchId).Patrons;
        }

        public bool IsBranchOpen(int branchId)
        {
            var currentTimeHour = DateTime.Now.Hour;
            var currentDayOfWeek = (int)DateTime.Now.DayOfWeek + 1;
            var hours = _context.BranchHours.Where(h => h.Branch.Id == branchId);
            var dayHours = hours.FirstOrDefault(h => h.DayOfWeek == currentDayOfWeek);

            return currentTimeHour < dayHours.CloseTime && currentTimeHour > dayHours.OpenTime;
        }
    }
}
