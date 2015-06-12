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
    public class PurchasesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        [HttpGet]
        public ActionResult Buy(int ShareId)
        {
            return View(db.StockShares.Where(x => x.ShareId == ShareId).ToList());
        }


        [HttpGet]
        public ActionResult BuyIndex()
        {
            if (TempData["errorInfo"] != null)
            {
                @ViewBag.errorInfo = TempData["errorInfo"].ToString();
            }

            if (TempData["successInfo"] != null)
            {
                @ViewBag.successInfo = TempData["successInfo"].ToString();
            }

            return View(db.StockShares.ToList());
        }

      


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BuyProcess([Bind(Include = "CurrentCode,TradedSharesAmount,Price")] string CurrentCode, string TradedSharesAmount, string Price)
        {


            double _Price;

 
    

            _Price =  double.Parse(Price, CultureInfo.InvariantCulture);


            int _PurchasedAmount = Convert.ToInt32(TradedSharesAmount);

            double PurchaseValue = Convert.ToDouble(_Price * _PurchasedAmount);


            using (var transaction = new TransactionScope())
            {


                var stock = db.StockShares.Where(s => s.Code.Equals(CurrentCode)).SingleOrDefault();

                var usId = User.Identity.GetUserId();
                var user = db.Users.Where(u => u.Id == usId).SingleOrDefault();




                //check if shares available
                if (stock.AmountAvailable >= _PurchasedAmount)
                {

                    //check if user has sufficient funds
                    if (user.MoneyAvailable >= PurchaseValue)
                    {


                        try
                        {

                            stock.AmountAvailable -= _PurchasedAmount;

                            user.MoneyAvailable -= PurchaseValue;


                            switch (CurrentCode)
                            {

                                case "FP":
                                    user.shares_FP += _PurchasedAmount;
                                    break;

                                case "FPL":
                                    user.shares_FPL += _PurchasedAmount;
                                    break;


                                case "FPC":
                                    user.shares_FPC += _PurchasedAmount;
                                    break;


                                case "PGB":
                                    user.shares_PGB += _PurchasedAmount;
                                    break;

                                case "FPA":
                                    user.shares_FPA += _PurchasedAmount;
                                    break;

                                case "DL24":
                                    user.shares_DL24 += _PurchasedAmount;
                                    break;

                            }


                            user.StockOperations.Add(new Operation
                            {

                                Code = stock.Code,
                                CreatedAt = System.DateTime.UtcNow,
                                AmountPurchased = _PurchasedAmount,
                                AmountSold = 0,
                                Price = _Price,
                                Value = PurchaseValue


                            });

                            db.SaveChanges();

                            transaction.Complete();

                            TempData["successInfo"] = "Transaction Complete!";



                        }
                        catch (Exception ex)
                        {
                            TempData["errorInfo"] = "An error occurred!";
                        }

                    }
                    else
                    {

                        TempData["errorInfo"] = "Insufficient funds!";

                    }

                }
                else
                {

                    TempData["errorInfo"] = "Shares not available";

                }

                return RedirectToAction("History", "Operations");

            }

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
