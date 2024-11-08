using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web.Mvc;

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
        //
        // GET: /Home/List
        [AllowAnonymous]
        public ActionResult List()
        {
            string categoryStr = Request.QueryString["categoryID"];
            using (SqlConnection connection = new SqlConnection(connectionString)) {
                connection.Open();
                return View("List");
            }
        }
        // GET: /Home/Info
        [AllowAnonymous]
        public ActionResult Info()
        {
            return View();
        }
    }
}
