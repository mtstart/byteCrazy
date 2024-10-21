using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
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
            // 这里可以使用一个默认的实现，或者留空
            // 注意：这不是最佳实践，但可以帮助你暂时解决错误
            _listingManagementServiceInterface = new AdminListMangementService(new ApplicationDbContext());
            
        }

        public ActionResult Index()
        {
            System.Diagnostics.Debug.WriteLine("Admin Index method called");

            _listingManagementServiceInterface.GetAllPostedProducts();

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
            return View("AdminPendingLists", _listingManagementServiceInterface.pendings);
        }

        public async Task<ActionResult> ActiveListings()
        {
            return View("AdminPendingLists", _listingManagementServiceInterface.actives);
        }

        public async Task<ActionResult> ViewDetails()
        {
            AdminListModels model = new AdminListModels
            {
                ProductID = "2",
                Title = "M2",
                Price = 800.50m,
                Description = "That's very good",
                SellerName = "Jane Smith",
                CreatedAt = DateTime.Now.AddDays(-1),
                Location = "Newcastle",
                Category = "Digtal",
                ImageUrls = new List<string> {
                        "https://cunchu.site/temp/byteCrazy/1.jpg",
                        "https://cunchu.site/temp/byteCrazy/4.jpg",
                        "https://cunchu.site/temp/byteCrazy/4.jpg",
                        "https://cunchu.site/temp/byteCrazy/4.jpg",
                        "https://cunchu.site/temp/byteCrazy/4.jpg"}
            };
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
        public async Task<ActionResult> VerifyListing(int id, bool isApproved, string rejectionReason = null)
        {
            return RedirectToAction("PendingListings");
       
        }

        public async Task<ActionResult> SoldItems()
        {
            var soldItems = await _listingManagementServiceInterface.GetSoldItemsAsync();
            return View(soldItems);
        }

    }
}

