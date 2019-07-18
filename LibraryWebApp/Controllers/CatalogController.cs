using LibraryData;
using LibraryWebApp.Models.Catalog;
using LibraryWebApp.Models.Checkout;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryWebApp.Controllers
{
    public class CatalogController: Controller
    {
        private ILibraryAsset _assets;
        private ICheckout _checkouts;

        public CatalogController(ILibraryAsset assets, ICheckout checkout)
        {
            _assets = assets;
            _checkouts = checkout;

        }
        public IActionResult Index()
        {
            var assetModels = _assets.GetAll();

            var listingResult = assetModels
                .Select(result => new AssetIndexListingModel
                {
                    Id = result.Id,
                    ImageUrl = result.ImageUrl,
                    AuthorOrDirectior = _assets.GetAuthorOrDirector(result.Id),
                    DeweyCallNumber = _assets.GetDeweyIndex(result.Id),
                    Title = result.Title,
                    Type = _assets.GetType(result.Id)
                });

            var model = new AssetIndexModel()
            {
                Assets = listingResult
            };

            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var asset = _assets.GetById(id);

            var currentHolds = _checkouts.GetCurrentHolds(id)
                .Select(a => new AssetHoldModel
                {
                    HoldPlaced = _checkouts.GetCurrentHoldPlaced(a.Id),
                    PatronName = _checkouts.GetCurrentHoldPatronName(a.Id)
                });

            var model = new AssetDetailModel
            {
                AssetId = id,
                Title = asset.Title,
                Year = asset.Year,
                Cost = asset.Cost,
                Status = asset.Status.Name,
                ImageUrl = asset.ImageUrl,
                AuthorOrDirector = _assets.GetAuthorOrDirector(id),
                CurrentLocation = _assets.GetCurrentLocation(id).Name,
                DeweyCallNumber = _assets.GetDeweyIndex(id),
                ISBN = _assets.GetIsbn(id),
                CheckoutHistory = _checkouts.GetCheckoutHistory(id),
                LastestCheckout = _checkouts.GetLastestCheckout(id),
                PatronName = _checkouts.GetCurrentCheckoutPatron(id),
                CurrentHolds = currentHolds

            };

            return View(model);
        }

        public IActionResult Checkout(int id) {
            var asset = _assets.GetById(id);

            var model = new CheckoutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkouts.IsCheckedOut(id)
            };

            return View(model);
        }
        public IActionResult CheckIn(int id) {

            _checkouts.CheckInItem(id);
            return RedirectToAction("Detail", new { id = id });

        }
        public IActionResult Hold(int id)
        {
            var asset =_assets.GetById(id);
            var model = new CheckoutModel
            {
                AssetId = id,
                ImageUrl = asset.ImageUrl,
                Title = asset.Title,
                LibraryCardId = "",
                IsCheckedOut = _checkouts.IsCheckedOut(id),
                HoldCount = _checkouts.GetCurrentHolds(id).Count()
            };
            return View(model);
        }

        public IActionResult MarkLost(int assetId) {
            _checkouts.MarkLost(assetId);
            return RedirectToAction("Detail", new { id = assetId });
        }
        public IActionResult MarkFound(int assetId)
        {
            _checkouts.MarkLost(assetId);
            return RedirectToAction("Detail", new { id = assetId });
        }
        [HttpPost]
        public IActionResult PlaceCheckout(int assetId, int libraryCardId)
        {
            _checkouts.CheckOutItem(assetId, libraryCardId);
            return RedirectToAction("Detail", new { id = assetId });
        }

        [HttpPost]
        public IActionResult PlaceHold(int assetId, int libraryCardId)
        {
            _checkouts.CheckInItem(assetId);
            return RedirectToAction("Detail", new { id = assetId });
        }
    }
}
