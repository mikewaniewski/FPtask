using FPtask.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FPtask.Controllers
{
    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();


 
         

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult GetChartData(object ShareTemplateCode)
        {

            string Code = ShareTemplateCode.ToString();


            var dataSet = db.StockShares.Where(x => x.Code.Equals(Code)).Select(x => x.PriceHistory).SingleOrDefault();


            var ds = dataSet.OrderBy(x => x.PublicationDate).Take(20).ToList();


            var labels = ds.Select(x => x.PublicationDate.ToString()).ToList();


            var values = ds.Select(x => x.Price).ToList();




            return Json(new { labels = labels, values = values });
        }

 


    }
}