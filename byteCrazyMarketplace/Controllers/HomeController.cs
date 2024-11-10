using System;
using System.IO;
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
            ViewBag.CategoryId = "sdsd";
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
        // POST: /Home/Edit
        [HttpPost]
        public ActionResult EditInfo(EditInfoModel model)
        {
            string updateQuery = "UPDATE [dbo].[Product] SET [description] = '" + model.description + "' WHERE [productID] = '" + model.productID + "'";
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
