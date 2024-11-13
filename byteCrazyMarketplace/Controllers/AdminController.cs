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
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace byteCrazy.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAdminListMangementService _listingManagementServiceInterface;
        private readonly ApplicationDbContext _context;

        public AdminController()
        {
            _context = new ApplicationDbContext();
            _listingManagementServiceInterface = new AdminListMangementService(new ApplicationDbContext());
            _listingManagementServiceInterface.GetAllPostedProducts();
        }

    
        private async Task<bool> IsAdmin()
        {
            var userId = User.Identity.GetUserId();
            var adminUser = await _context.AdminUserList
                .FirstOrDefaultAsync(a => a.userID == userId && a.status == "Active");

            if (adminUser == null) return false;



            return true;
        }

        public async Task<ActionResult> Index()
        {
            System.Diagnostics.Debug.WriteLine("Admin Index method called");

            if (!await IsAdmin())
            {
                return RedirectToAction("UserCenter", "Account");
            }

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
            if (!await IsAdmin())
            {
                return RedirectToAction("UserCenter", "Account");
            }
            return View("AdminTotalLists", _listingManagementServiceInterface.alls);
        }

        public async Task<ActionResult> PendingListings()
        {
            if (!await IsAdmin())
            {
                return RedirectToAction("UserCenter", "Account");
            }
            return View("AdminPendingLists", _listingManagementServiceInterface.pendings);
        }

        public async Task<ActionResult> ActiveListings()
        {
            if (!await IsAdmin())
            {
                return RedirectToAction("UserCenter", "Account");
            }
            return View("AdminActivityLists", _listingManagementServiceInterface.actives);
        }

        public async Task<ActionResult> SoldItems()
        {
            if (!await IsAdmin())
            {
                return RedirectToAction("UserCenter", "Account");
            }
            return View("AdminSoldedLists", _listingManagementServiceInterface.solds);
        }

        public async Task<ActionResult> ViewDetails(string productID, string isApproved, string rejectionReason)
        {
            if (!await IsAdmin())
            {
                return RedirectToAction("UserCenter", "Account");
            }
            if (isApproved == "true")
            {
                _listingManagementServiceInterface.ApproveProduct(productID);
            }
            if (isApproved == "false")
            {
                _listingManagementServiceInterface.RejectProduct(productID, rejectionReason);
            }
            Console.WriteLine($"Hello, {productID}!");
            AdminListModels model = _listingManagementServiceInterface.SearchProdunct(productID);
            return View("AdminVerifyLists", model);
        }

        public async Task<ActionResult> VerifyListing(int id)
        {
            if (!await IsAdmin())
            {
                return RedirectToAction("UserCenter", "Account");
            }
            var listing = await _listingManagementServiceInterface.GetListingByIdAsync(id);
            if (listing == null)
            {
                return HttpNotFound();
            }
            return View(listing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyListing(string productID, bool isApproved, string rejectionReason)
        {
            if (!await IsAdmin())
            {
                return RedirectToAction("UserCenter", "Account");
            }
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}