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
                TotalListings = 0, // 替换为实际数据
                PendingVerification = 0, // 替换为实际数据
                ActiveListings = _listingManagementServiceInterface.solds.Count, // 替换为实际数据
                SoldListings = _listingManagementServiceInterface.solds.Count // 替换为实际数据
            };
            return View("AdminDashboard", viewModel);

        }

        public async Task<ActionResult> TotalListings()
        {
           _listingManagementServiceInterface.GetAllPostedProducts();

            return null;
        }

        public async Task<ActionResult> PendingListings()
        {

            var pendingListings = new List<AdminListModels>
            {
                new AdminListModels
                {
                    ProductID = "1",
                    Title = "Vintage Guitar",
                    Price = 1200.00m,
                    SellerName = "John Doe",
                    CreatedAt = DateTime.Now.AddDays(-2),
                    ImageUrls = new List<string> { "https://cunchu.site/temp/byteCrazy/1.jpg", "https://cunchu.site/temp/byteCrazy/3.jpg" }
                },
                new AdminListModels
                {
                    ProductID = "2",
                    Title = "Mountain Bike",
                    Price = 800.50m,
                    SellerName = "Jane Smith",
                    CreatedAt = DateTime.Now.AddDays(-1),
                    ImageUrls = new List<string> { "https://cunchu.site/temp/byteCrazy/1.jpg", "https://cunchu.site/temp/byteCrazy/4.jpg" }
                },
                new AdminListModels
                {
                    ProductID = "3",
                    Title = "Mountain Carmine",
                    Price = 800.50m,
                    SellerName = "Jae Willim",
                    CreatedAt = DateTime.Now.AddDays(-1),
                    ImageUrls = new List<string> {
                        "https://cunchu.site/temp/byteCrazy/1.jpg",
                        "https://cunchu.site/temp/byteCrazy/4.jpg",
                        "https://cunchu.site/temp/byteCrazy/4.jpg",
                        "https://cunchu.site/temp/byteCrazy/4.jpg",
                        "https://cunchu.site/temp/byteCrazy/4.jpg"}
                },
            };

            return View("AdminPendingLists", pendingListings);
        }

        public async Task<ActionResult> ActiveListings()
        {

            var pendingListings = new List<AdminListModels>
            {
                new AdminListModels
                {
                    ProductID = "2",
                    Title = "M2",
                    Price = 800.50m,
                    SellerName = "Jane Smith",
                    CreatedAt = DateTime.Now.AddDays(-1),
                    ImageUrls = new List<string> { "https://cunchu.site/temp/byteCrazy/5.jpg" }
                },
                new AdminListModels
                {
                    ProductID = "1",
                    Title = "Mo3",
                    Price = 800.50m,
                    SellerName = "Jae Willim",
                    CreatedAt = DateTime.Now.AddDays(-1),
                    ImageUrls = new List<string> {
                        "https://cunchu.site/temp/byteCrazy/3.jpg",
                        "https://cunchu.site/temp/byteCrazy/3.jpg",
                        "https://cunchu.site/temp/byteCrazy/4.jpg",
                        "https://cunchu.site/temp/byteCrazy/4.jpg",
                        "https://cunchu.site/temp/byteCrazy/4.jpg"}
                },
            };

            return View("AdminPendingLists", pendingListings);
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

