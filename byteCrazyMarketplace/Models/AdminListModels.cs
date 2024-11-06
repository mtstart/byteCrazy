using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using byteCrazy.Interface;

namespace byteCrazy.Models
{
    public class AdminDashboardModels
    {
        public int TotalListings { get; set; }
        public int PendingVerification { get; set; }
        public int ActiveListings { get; set; }
        public int SoldListings { get; set; }
    }

    public class AdminListModels
	{
        [Key]
        public string ProductID { get; set; }

        [Key]
        public string UserID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(1000)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 1000000)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }


        [Required]
        public string SellerId { get; set; }

        [Required]
        [StringLength(50)]
        public string SellerName { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public string StatusString { get; set; }

        //[Required]
        //public ListingStatus Status { get; set; }

        // 将枚举逻辑封装在模型中
        public ListingStatus Status
        {
            get
            {
                // 将字符串转换为枚举
                if (Enum.TryParse(StatusString, out ListingStatus status))
                {
                    return status;
                }
                else
                {
                    // 处理未知状态，例如返回默认值
                    return ListingStatus.pending; // 可以根据需求调整
                }
            }
            set
            {
                // 将枚举转换为字符串并存储
                StatusString = value.ToString();
            }
        }

        public List<string> ImageUrls { get; set; }

        [StringLength(50)]
        public string VerifiedBy { get; set; }

        public DateTime? VerifiedAt { get; set; }

        [StringLength(255)]
        public string RejectionReason { get; set; }

        public DateTime? SoldAt { get; set; }

        [StringLength(50)]
        public string Category { get; set; }

        [StringLength(100)]
        public string Location { get; set; }

        public bool IsNegotiable { get; set; }

        public int ViewCount { get; set; }

        public DateTime? LastModifiedAt { get; set; }

        [StringLength(50)]
        public string LastModifiedBy { get; set; }

        [NotMapped]
        public TimeSpan Age => DateTime.Now - CreatedAt;

        public AdminListModels()
        {
            CreatedAt = DateTime.Now;
            Status = ListingStatus.pending;
            ViewCount = 0;
        }

        public void Verify(string adminId)
        {
            Status = ListingStatus.active;
            VerifiedBy = adminId;
            VerifiedAt = DateTime.Now;
            LastModifiedAt = DateTime.Now;
            LastModifiedBy = adminId;
        }

        public void Reject(string adminId, string reason)
        {
            Status = ListingStatus.rejected;
            VerifiedBy = adminId;
            VerifiedAt = DateTime.Now;
            RejectionReason = reason;
            LastModifiedAt = DateTime.Now;
            LastModifiedBy = adminId;
        }

        public void MarkAsSold()
        {
            Status = ListingStatus.sold;
            SoldAt = DateTime.Now;
            LastModifiedAt = DateTime.Now;
        }

        public void IncrementViewCount()
        {
            ViewCount++;
        }

        public void Update(string title, string description, decimal price, string category, bool isNegotiable, string location, string modifiedBy)
        {
            Title = title;
            Description = description;
            Price = price;
            Category = category;
            IsNegotiable = isNegotiable;
            Location = location;
            LastModifiedAt = DateTime.Now;
            LastModifiedBy = modifiedBy;
        }

        public bool CanBeEditedBy(string userId)
        {
            return SellerId == userId && Status != ListingStatus.sold;
        }
    }

}

