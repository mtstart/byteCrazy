using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using System.Xml.Linq;
using byteCrazy.Interface;
using byteCrazy.Models;

namespace byteCrazy.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminListMangementService _listingManagementServiceInterface;

        public AdminController()
        {
            _listingManagementServiceInterface = new AdminListMangementService(new ApplicationDbContext());
            _listingManagementServiceInterface.GetAllPostedProducts();

        }

        public ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("Admin Index method called");

            var viewModel = new AdminDashboardModels
            {
                TotalListings = _listingManagementServiceInterface.alls.Count, 
                PendingVerification = _listingManagementServiceInterface.pendings.Count, 
                ActiveListings = _listingManagementServiceInterface.actives.Count,
                SoldListings = _listingManagementServiceInterface.solds.Count 
            };
            return View("AdminDashboard", viewModel);

        }

        public async Task<ActionResult> TotalListings()
        {
           return View("AdminTotalLists", _listingManagementServiceInterface.alls);
        }

        public async Task<ActionResult> PendingListings()
        {
            return View("AdminTotalLists", _listingManagementServiceInterface.pendings);
        }

        public async Task<ActionResult> ActiveListings()
        {
            return View("AdminTotalLists", _listingManagementServiceInterface.actives);
        }

        public async Task<ActionResult> SoldItems()
        {
            return View("AdminTotalLists", _listingManagementServiceInterface.solds);
        }

        public async Task<ActionResult> ViewDetails(string id)
        {
            Console.WriteLine($"Hello, {id}!");
            AdminListModels model = _listingManagementServiceInterface.SearchProdunct(id);
            return View("AdminVerifyLists", model);
        }

            public async Task<ActionResult> VerifyListing(int id)
        {
            var listing = await _listingManagementServiceInterface.GetListingByIdAsync(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            return View(listing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyListing(string productID, bool isApproved, string rejectionReason)
        {
            if (isApproved)
            {
                _listingManagementServiceInterface.ApproveProduct(productID);
            }

            else
            {
                _listingManagementServiceInterface.RejectProduct(productID, rejectionReason);
            }

            if (Request.UrlReferrer != null)
            {
                return Redirect(Request.UrlReferrer.ToString());
            }
            else
            {
                return RedirectToAction("Index"); 
            }
        }
    }
}

