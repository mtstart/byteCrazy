using System;
using System.IO;
using System.Text;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web.Mvc;
using byteCrazy.Models;

namespace byteCrazy.Controllers
{
    // 创建视图模型
    public class IndexViewModel
    {
        public string CategoryId { get; set; }
    }
    public class HomeController : Controller
    {
        private string connectionString = "Server=1.94.181.181,1433;Database=byteCrazy;User Id=admin;Password=XQNQ0MEUL9yrtyhmlfe1866;";
        
        public ActionResult Index()
        {
            return View();
        }



        // POST: /Home/Upload
        [HttpPost]
        public ActionResult Upload(ImageUploadViewModel model)
        {
            if (model.UploadedImage != null && model.UploadedImage.ContentLength > 0)
            {
                // 确定保存上传文件的路径
                string uploadPath = Server.MapPath("~/UploadedImages");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // 保存文件
                string fileName = Path.GetFileName(model.UploadedImage.FileName);
                string filePath = Path.Combine(uploadPath, fileName);
                model.UploadedImage.SaveAs(filePath);

                string updateQuery = "UPDATE [dbo].[Product] SET [imgUrl] = '/UploadedImages/" + fileName + "' WHERE [productID] = '" + model.productID + "'";
                // 更新数据
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // 打开数据库连接
                    connection.Open();

                    // 创建 SqlCommand 对象
                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        Console.WriteLine(updateQuery);
                        command.ExecuteNonQuery();
                    }
                }
            }
            return RedirectToAction("Info", new { productID = model.productID });
        }
        // POST: /Home/EditInfo
        [HttpPost]
        public ActionResult EditInfo(EditInfoModel model)
        {
            string updateQuery = "UPDATE [dbo].[Product] SET [description] = '" + model.description + "' AND [locationValue] = '" + model.locationValue + "' AND [priceValue] = '" + model.priceValue + "' AND [title] = '" + model.title + "' WHERE [productID] = '" + model.productID + "'";
            // 更新数据
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // 打开数据库连接
                connection.Open();

                // 创建 SqlCommand 对象
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    Console.WriteLine(updateQuery);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Info", new { productID = model.productID });
        }

        // POST: /Home/Report
        [HttpPost]
        public ActionResult Report(EditInfoModel model)
        {
            string updateQuery = "UPDATE [dbo].[Product] SET [status] = 'pending' WHERE [productID] = '" + model.productID + "'";
            // 更新数据
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // 打开数据库连接
                connection.Open();

                // 创建 SqlCommand 对象
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    Console.WriteLine(updateQuery);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("List", new { });
        }
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder result = new StringBuilder(length);
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[random.Next(chars.Length)]);
            }

            return result.ToString();
        }
        // Get: /Home/AddNew
        [AllowAnonymous]
        public ActionResult AddNew()
        {
            string productID = GenerateRandomString(8);
            
            string updateQuery = "INSERT INTO [dbo].[Product] ([productID], [title], [description], [categoryID], [location], [price], [imgUrl], [sellerID], [buyerID], [status], [postedDate], [purchaseDate]) OUTPUT INSERTED.[productID], INSERTED.[title], INSERTED.[description], INSERTED.[categoryID], INSERTED.[location], INSERTED.[price], INSERTED.[imgUrl], INSERTED.[sellerID], INSERTED.[buyerID], INSERTED.[status], INSERTED.[postedDate], INSERTED.[purchaseDate] VALUES (N'" + productID + "', N'title', N'description', N'CAT003', N'location', 1000, N'/UploadedImages/20241112020656.png', N'" + User.Identity.GetUserId() +"', NULL, N'pending', '2024-11-11 01:00:00.000', NULL)";
            // 更新数据
            Console.WriteLine(updateQuery);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // 打开数据库连接
                connection.Open();

                // 创建 SqlCommand 对象
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    Console.WriteLine(updateQuery);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Info", new { productID = productID });
        }
        //
        // GET: /Home/List
        [AllowAnonymous]
        public ActionResult List()
        {
            return View("List");
        }
        // GET: /Home/Info
        [AllowAnonymous]
        public ActionResult Info()
        {
            return View();
        }
    }
}
