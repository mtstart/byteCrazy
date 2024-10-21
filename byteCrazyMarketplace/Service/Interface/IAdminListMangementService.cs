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
        // 获取待验证的帖子列表
        Task<IEnumerable<AdminListModels>> GetPendingListingsAsync(int page = 1, int pageSize = 20);

        // 获取已售出的物品列表
        Task<IEnumerable<AdminListModels>> GetSoldItemsAsync(int page = 1, int pageSize = 20);

        // 获取单个帖子的详细信息
        Task<AdminListModels> GetListingByIdAsync(int id);

        // 验证帖子
        Task<bool> VerifyListingAsync(int listingId, string adminId);

        // 拒绝帖子
        Task<bool> RejectListingAsync(int listingId, string adminId, string rejectionReason);

        // 标记帖子为已售出
        Task<bool> MarkListingAsSoldAsync(int listingId, string sellerId);

        // 获取帖子统计信息
        Task<ListingStats> GetListingStatsAsync();

        // 搜索帖子
        Task<IEnumerable<AdminListModels>> SearchListingsAsync(string searchTerm, ListingStatus? status = null, int page = 1, int pageSize = 20);

        // 更新帖子信息
        Task<bool> UpdateListingAsync(int listingId, AdminListModels updatedListing);

        // 删除帖子
        Task<bool> DeleteListingAsync(int listingId);

        // 获取某个卖家的所有帖子
        Task<IEnumerable<AdminListModels>> GetListingsBySellerAsync(string sellerId, int page = 1, int pageSize = 20);

        // 批量验证帖子
        Task<int> BatchVerifyListingsAsync(IEnumerable<int> listingIds, string adminId);

        // 获取最近的活动日志
        Task<IEnumerable<ListingActivityLog>> GetRecentActivityLogsAsync(int count = 50);
    }

    public enum ListingStatus
    {
        Pending,
        Active,
        Rejected,
        Sold
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

