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
using Microsoft.SqlServer.Server;
using System.Xml.Linq;

namespace byteCrazy.Controllers
{
    // Create view model
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
                // check image path, make sure is in project folder
                string uploadPath = Server.MapPath("~/UploadedImages");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // save image
                string fileName = Path.GetFileName(model.UploadedImage.FileName);
                string filePath = Path.Combine(uploadPath, fileName);
                model.UploadedImage.SaveAs(filePath);

                string updateQuery = "UPDATE [dbo].[Product] SET [imgUrl] = '/UploadedImages/" + fileName + "' WHERE [productID] = '" + model.productID + "'";
                // Update data
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Create connection destination
                    connection.Open();

                    // Create connection destination
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
            string updateQuery = "UPDATE [dbo].[Product] SET [description] = '" + model.description + "' , [location] = '" + model.locationValue + "' , [price] = '" + model.priceValue + "' , [title] = '" + model.title + "' WHERE [productID] = '" + model.productID + "'";
            // Update data
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open connection
                connection.Open();

                // Create connection destination
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    Console.WriteLine(updateQuery);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Info", new { productID = model.productID });
        }

        // POST: /Home/SaveLike
        [HttpPost]
        public ActionResult SaveLike(SaveLikeModel model)
        {
            string updateQuery = "INSERT INTO [dbo].[SavedProducts] (userID, productID, createdDate) VALUES (" + model.userID + ", '" + model.productID + "', '" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "');";
            // Update data
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open connection
                connection.Open();

                // Create connection destination
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    Console.WriteLine(updateQuery);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Info", new { productID = model.productID });
        }

        // POST: /Home/DeleteLike
        [HttpPost]
        public ActionResult DeleteLike(SaveLikeModel model)
        {
            string updateQuery = "DELETE FROM [dbo].[SavedProducts] WHERE CONVERT(NVARCHAR(MAX), [userID]) = '" + model.userID + "' AND CONVERT(NVARCHAR(MAX), [productID]) = '" + model.productID + "'";
            // Update data
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open connection
                connection.Open();

                // Create connection destination
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
            // Update data
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Open connection
                connection.Open();

                // Create connection destination
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

        // Post: /Home/Create
        [AllowAnonymous]
        public ActionResult Create(EditInfoModel model)
        {
            string productID = GenerateRandomString(8);

            string insertQuery = "";
            insertQuery = "INSERT INTO [dbo].[Product] VALUES (";
            insertQuery += "N'" + productID + "', ";
            insertQuery += "N'" + model.title + "', ";
            insertQuery += "N'" + model.description + "', ";
            insertQuery += "N'" + model.categoryValue + "', ";
            insertQuery += "N'" + model.locationValue + "', ";
            insertQuery += "N'" + model.priceValue + "', ";
            insertQuery += "N'" + model.imgUrl + "', ";
            insertQuery += "N'" + User.Identity.GetUserId() + "', ";
            insertQuery += "NULL" + ", ";
            insertQuery += "N'" + "active" + "', ";
            insertQuery += "getdate()" + ", ";
            insertQuery += "NULL" + ", ";
            insertQuery += "NULL";
            insertQuery += ")";


            // Insert data
            Console.WriteLine(insertQuery);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Create connection
                connection.Open();

                // Create connection destination
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    Console.WriteLine(insertQuery);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Info", new { productID = productID });

        }

        // Get: /Home/AddImage
        [AllowAnonymous]
        public ActionResult AddImage(ImageUploadViewModel model)
        {
            string filePath = "";
            if (model.UploadedImage != null && model.UploadedImage.ContentLength > 0)
            {
                // check image path, make sure is in project folder
                string uploadPath = Server.MapPath("~/UploadedImages");
                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                // save image
                string fileName = Path.GetFileName(model.UploadedImage.FileName);
                filePath = Path.Combine(uploadPath, fileName);
                model.UploadedImage.SaveAs(filePath);

                fileName = "/UploadedImages/" + fileName;
                ViewBag.showImg = fileName;

                Console.WriteLine(fileName);

            }

            return View("AddNew");
        }

        // Get: /Home/AddNew
        [AllowAnonymous]
        public ActionResult AddNew()
        {
            return View();
        }
        
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
