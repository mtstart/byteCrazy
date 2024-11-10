using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using byteCrazy.Models;

namespace byteCrazy.Interface
{
	public interface IAdminListMangementService
    {
        //All
        List<AdminListModels> alls { get; set; }

        //Pending
        List<AdminListModels> pendings { get; set; }

        //Sold
        List<AdminListModels> solds { get; set; }

        //Active
        List<AdminListModels> actives { get; set; }

        // 获取所有产品信息
        void GetAllPostedProducts();

        // 获取对应产品信息
        //Task<AdminListModels>  SearchProdunct(string productId);
        AdminListModels SearchProdunct(string productId);

        //ApproveProduct
        void ApproveProduct(string productId);

        //RejectProduct
        void RejectProduct(string productId, string rejectionReason);

        // GetPendingListingsAsync
        Task<IEnumerable<AdminListModels>> GetPendingListingsAsync(int page = 1, int pageSize = 20);

        // GetPendingListingsAsync
        Task<IEnumerable<AdminListModels>> GetSoldItemsAsync(int page = 1, int pageSize = 20);

        // GetListingByIdAsync
        Task<AdminListModels> GetListingByIdAsync(int id);

        // VerifyListingAsync
        Task<bool> VerifyListingAsync(int listingId, string adminId);

        // RejectListingAsync
        Task<bool> RejectListingAsync(int listingId, string adminId, string rejectionReason);

        // MarkListingAsSoldAsync
        Task<bool> MarkListingAsSoldAsync(int listingId, string sellerId);

        // GetListingStatsAsync
        Task<ListingStats> GetListingStatsAsync();

        // SearchListingsAsync
        Task<IEnumerable<AdminListModels>> SearchListingsAsync(string searchTerm, ListingStatus? status = null, int page = 1, int pageSize = 20);

        // UpdateListingAsync
        Task<bool> UpdateListingAsync(int listingId, AdminListModels updatedListing);

        // DeleteListingAsync
        Task<bool> DeleteListingAsync(int listingId);

        // GetListingsBySellerAsync
        Task<IEnumerable<AdminListModels>> GetListingsBySellerAsync(string sellerId, int page = 1, int pageSize = 20);

        // BatchVerifyListingsAsync
        Task<int> BatchVerifyListingsAsync(IEnumerable<int> listingIds, string adminId);

        // GetRecentActivityLogsAsync
        Task<IEnumerable<ListingActivityLog>> GetRecentActivityLogsAsync(int count = 50);
    }

    public enum ListingStatus
    {
        pending,
        active,
        rejected,
        sold
    }

    public class ListingStats
    {
        public int TotalListings { get; set; }
        public int PendingVerification { get; set; }
        public int ActiveListings { get; set; }
        public int SoldListings { get; set; }
        public int RejectedListings { get; set; }
    }

    public class ListingActivityLog
    {
        public int Id { get; set; }
        public int ListingId { get; set; }
        public string Action { get; set; }
        public string PerformedBy { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }
    }

   
}

