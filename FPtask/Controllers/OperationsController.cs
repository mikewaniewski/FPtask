using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FPtask.Models;
using System.Transactions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Globalization;
 

namespace FPtask.Controllers
{

    [Authorize]
    public class OperationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


          

 




        [AllowAnonymous]
        public ActionResult SaveHistoryPrices(object pricesJson)
        {


            using (ApplicationDbContext dbc = new ApplicationDbContext())
            {
                var jo = JObject.Parse(pricesJson.ToString());
                var PublicationDate = jo["PublicationDate"].ToString();
                DateTime _PublicationDate = Convert.ToDateTime(PublicationDate);

                int count = jo["Items"].Count();

                for (int i = 0; i < count; i++)
                {
                    string Code = jo["Items"][i]["Code"].ToString();
                    string Price = jo["Items"][i]["Price"].ToString();
                    double _Price = 0;


                    _Price = double.Parse(Price, CultureInfo.InvariantCulture);

                    var stockShareTemplate = dbc.StockShares.Where(s => s.Code.Equals(Code)).Select(s => s).SingleOrDefault();


                    if (stockShareTemplate != null)
                    {
                        var chk = stockShareTemplate.PriceHistory.Where(d => d.PublicationDate == _PublicationDate);


                        if (chk.Count() == 0)
                        {
                            stockShareTemplate.PriceHistory.Add(
                             new SharePriceHistory
                             {

                                 PublicationDate = _PublicationDate,
                                 Price = _Price,
                                 UpdatersIp = Request.ServerVariables["REMOTE_ADDR"]
                             });


                            dbc.SaveChanges();
                        }



                    }

                }
                
            }
                return Json(new { status = "Success", message = "Prices Updated Successfully" });
          

        }





        public ActionResult History()
        {

            if (TempData["errorInfo"] != null)
            {
                @ViewBag.errorInfo = TempData["errorInfo"].ToString();
            }

            if (TempData["successInfo"] != null)
            {
                @ViewBag.successInfo = TempData["successInfo"].ToString();
            }

            var usId = User.Identity.GetUserId();
            var user = db.Users.Where(u => u.Id == usId).SingleOrDefault();

            var model = user.StockOperations.OrderByDescending(s=>s.CreatedAt).Select(s => s).ToList();

            return View(model);

        }

 
         
         
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
