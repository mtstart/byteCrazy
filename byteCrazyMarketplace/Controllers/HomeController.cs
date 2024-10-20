using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace byteCrazy.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        //
        // GET: /Home/List
        [AllowAnonymous]
        public ActionResult List()
        {
            return View();
        }
        // GET: /Home/Info
        [AllowAnonymous]
        public ActionResult Info()
        {
            return View();
        }
    }
}
