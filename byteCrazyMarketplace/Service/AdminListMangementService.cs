using System.Collections.Generic;
using System.Threading.Tasks;
using byteCrazy.Models;
using System.Data.Entity;
using System.Linq;
using byteCrazy.Interface;
using System.Data.SqlClient;
using System;
using System.Configuration;

namespace byteCrazy.Interface
{
    //TODO: 后续修改为全局
    /*
        1.94.181.181
        端口
        1433
        验证方式
        SQL Server验证
        用户名
        admin
        密码
        XQNQ0MEUL9yrtyhmlfe1866
     */
    public class AdminListMangementService : IAdminListMangementService
    {
        private string connectionString = "Server=1.94.181.181,1433;Database=byteCrazy;User Id=admin;Password=XQNQ0MEUL9yrtyhmlfe1866;";
        
        private readonly ApplicationDbContext _context;

        public AdminListMangementService(ApplicationDbContext context)
        {
            _context = context;
        }

        //All
        public List<AdminListModels> alls { get; set; }

        //Pending
        public List<AdminListModels> pendings { get; set; }

        //Sold
        public List<AdminListModels> solds { get; set; }

        //Active
        public List<AdminListModels> actives { get; set; }

        //GetAllPostedProducts
        public void GetAllPostedProducts()
        { 
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Product";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        alls = new List<AdminListModels>();
                        pendings = new List<AdminListModels>();
                        solds = new List<AdminListModels>();
                        actives = new List<AdminListModels>();

                        while (reader.Read())
                        {
                            var product = new AdminListModels
                            {
                                UserID = reader["sellerID"].ToString(),
                                Title = reader["title"].ToString(),
                                ProductID = reader["productID"].ToString(),
                                CreatedAt = Convert.ToDateTime(reader["postedDate"]),
                                StatusString = reader["status"].ToString(),
                                ImageUrls = new List<string> { reader["imgUrl"].ToString() }
                            };

                            alls.Add(product);

                            switch (product.Status)
                            {
                                case ListingStatus.sold:
                                    solds.Add(product);
                                    break;
                                case ListingStatus.active:
                                    actives.Add(product);
                                    break;
                                case ListingStatus.pending:
                                    actives.Add(product);
                                    break;
                                default:
                                    break;
                            }

                        }
                    }
                }
            }
        }

        //SearchProdunct
        public AdminListModels SearchProdunct(string productId)
        {
            AdminListModels model = new AdminListModels();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Product WHERE ProductID = @ProductID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductID", productId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model = new AdminListModels
                            {
                                UserID = reader["sellerID"].ToString(),
                                Title = reader["title"].ToString(),
                                ProductID = reader["productID"].ToString(),
                                Location = reader["location"].ToString(),
                                //Category = reader["category"].ToString(),
                                Description = reader["description"].ToString(),
                                Price = Convert.ToDecimal(reader["price"]),
                                CreatedAt = Convert.ToDateTime(reader["postedDate"]),
                                StatusString = reader["status"].ToString(),
                                ImageUrls = new List<string> { reader["imgUrl"].ToString() }
                            };
                        }
                    }
                }
            }

            return model;
        }

        public async Task<IEnumerable<AdminListModels>> GetPendingListingsAsync(int page = 1, int pageSize = 20)
        {
            return await _context.Listings
                .Where(l => l.Status == ListingStatus.pending)
                .OrderByDescending(l => l.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<IEnumerable<AdminListModels>> GetSoldItemsAsync(int page = 1, int pageSize = 20)
        {
            return await _context.Listings
                .Where(l => l.Status == ListingStatus.sold)
                .OrderByDescending(l => l.SoldAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<AdminListModels> GetListingByIdAsync(int id)
        {
            return await _context.Listings.FindAsync(id);
        }

        public async Task<bool> VerifyListingAsync(int listingId, string adminId)
        {
            var listing = await _context.Listings.FindAsync(listingId);
            if (listing == null)
            {
                return false;
            }

            listing.Verify(adminId);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectListingAsync(int listingId, string adminId, string rejectionReason)
        {
            var listing = await _context.Listings.FindAsync(listingId);
            if (listing == null)
            {
                return false;
            }

            listing.Reject(adminId, rejectionReason);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> MarkListingAsSoldAsync(int listingId, string sellerId)
        {
            var listing = await _context.Listings.FindAsync(listingId);
            if (listing == null || listing.SellerId != sellerId)
            {
                return false;
            }

            listing.MarkAsSold();
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ListingStats> GetListingStatsAsync()
        {
            var stats = new ListingStats
            {
                TotalListings = await _context.Listings.CountAsync(),
                PendingVerification = await _context.Listings.CountAsync(l => l.Status == ListingStatus.pending),
                ActiveListings = await _context.Listings.CountAsync(l => l.Status == ListingStatus.active),
                SoldListings = await _context.Listings.CountAsync(l => l.Status == ListingStatus.sold),
                RejectedListings = await _context.Listings.CountAsync(l => l.Status == ListingStatus.rejected)
            };

            return stats;
        }

        public async Task<IEnumerable<AdminListModels>> SearchListingsAsync(string searchTerm, ListingStatus? status = null, int page = 1, int pageSize = 20)
        {
            var query = _context.Listings.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(l => l.Title.Contains(searchTerm) || l.Description.Contains(searchTerm));
            }

            if (status.HasValue)
            {
                query = query.Where(l => l.Status == status.Value);
            }

            return await query
                .OrderByDescending(l => l.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<bool> UpdateListingAsync(int listingId, AdminListModels updatedListing)
        {
            var listing = await _context.Listings.FindAsync(listingId);
            if (listing == null)
            {
                return false;
            }

            listing.Update(
                updatedListing.Title,
                updatedListing.Description,
                updatedListing.Price,
                updatedListing.Category,
                updatedListing.IsNegotiable,
                updatedListing.Location,
                updatedListing.LastModifiedBy
            );

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteListingAsync(int listingId)
        {
            var listing = await _context.Listings.FindAsync(listingId);
            if (listing == null)
            {
                return false;
            }

            _context.Listings.Remove(listing);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AdminListModels>> GetListingsBySellerAsync(string sellerId, int page = 1, int pageSize = 20)
        {
            return await _context.Listings
                .Where(l => l.SellerId == sellerId)
                .OrderByDescending(l => l.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> BatchVerifyListingsAsync(IEnumerable<int> listingIds, string adminId)
        {

            return 0;
        }

        public async Task<IEnumerable<ListingActivityLog>> GetRecentActivityLogsAsync(int count = 50)
        {
            return await _context.ListingActivityLogs
                .OrderByDescending(l => l.Timestamp)
                .Take(count)
                .ToListAsync();
        }
    }
}

