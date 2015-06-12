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
    public class SalesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        [HttpGet]
        public ActionResult Sell(int ShareId)
        {

            var shares = db.StockShares.Where(x => x.ShareId == ShareId).SingleOrDefault();

            var usId = User.Identity.GetUserId();
            var user = db.Users.Where(u => u.Id == usId).SingleOrDefault();


            string shareCode = shares.Code;

            int AmountAvailableForUser = 0;

            switch (shareCode)
            {

                case "FP":
                    AmountAvailableForUser = user.shares_FP;
                    break;

                case "FPL":
                    AmountAvailableForUser = user.shares_FPL;
                    break;


                case "FPC":
                    AmountAvailableForUser = user.shares_FPC;
                    break;


                case "PGB":
                    AmountAvailableForUser = user.shares_PGB;
                    break;

                case "FPA":
                    AmountAvailableForUser = user.shares_FPA;
                    break;

                case "DL24":
                    AmountAvailableForUser = user.shares_DL24;
                    break;

            }


            List<SellViewModel> model = new List<SellViewModel>();

            model.Add(new SellViewModel
            {
                ShareId = shares.ShareId,
                Code = shareCode,
                Name = shares.Name,
                AmountAvailableForUser = AmountAvailableForUser
            });

            return View(model.ToList());
        }



        [HttpGet]
        public ActionResult SellIndex()
        {
            if (TempData["errorInfo"] != null)
            {
                @ViewBag.errorInfo = TempData["errorInfo"].ToString();
            }

            if (TempData["successInfo"] != null)
            {
                @ViewBag.successInfo = TempData["successInfo"].ToString();
            }


            var shares = db.StockShares.ToList();

            List<SellViewModel> modelList = new List<SellViewModel>();

            foreach (var s in shares)
            {

                var usId = User.Identity.GetUserId();
                var user = db.Users.Where(u => u.Id == usId).SingleOrDefault();

                int AmountAvailableForUser = 0;
                switch (s.Code)
                {

                    case "FP":
                        AmountAvailableForUser = user.shares_FP;
                        break;

                    case "FPL":
                        AmountAvailableForUser = user.shares_FPL;
                        break;


                    case "FPC":
                        AmountAvailableForUser = user.shares_FPC;
                        break;


                    case "PGB":
                        AmountAvailableForUser = user.shares_PGB;
                        break;

                    case "FPA":
                        AmountAvailableForUser = user.shares_FPA;
                        break;

                    case "DL24":
                        AmountAvailableForUser = user.shares_DL24;
                        break;

                }


                modelList.Add(new SellViewModel
                {
                    ShareId = s.ShareId,
                    Code = s.Code,
                    Name = s.Name,
                    AmountAvailableForUser = AmountAvailableForUser
                });


            }



            return View(modelList);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SellProcess([Bind(Include = "CurrentCode,TradedSharesAmount,Price")] string CurrentCode, string TradedSharesAmount, string Price)
        {

            double _Price;
 

            _Price = double.Parse(Price, CultureInfo.InvariantCulture);

            int _SoldAmount = Convert.ToInt32(TradedSharesAmount);

            double SellValue = Convert.ToDouble(_Price * _SoldAmount);


            using (var transaction = new TransactionScope())
            {

                var stock = db.StockShares.Where(s => s.Code.Equals(CurrentCode)).SingleOrDefault();


                var usId = User.Identity.GetUserId();
                var user = db.Users.Where(u => u.Id == usId).SingleOrDefault();



                stock.AmountAvailable += _SoldAmount;

                user.MoneyAvailable += SellValue;

                int remainingShares = 0;


                switch (CurrentCode)
                {

                    case "FP":

                        user.shares_FP -= _SoldAmount;
                        remainingShares = user.shares_FP;
                        break;

                    case "FPL":

                        user.shares_FPL -= _SoldAmount;
                        remainingShares = user.shares_FPL;
                        break;

                    case "FPC":

                        user.shares_FPC -= _SoldAmount;
                        remainingShares = user.shares_FPC;
                        break;

                    case "PGB":

                        user.shares_PGB -= _SoldAmount;
                        remainingShares = user.shares_PGB;
                        break;

                    case "FPA":

                        user.shares_FPA -= _SoldAmount;
                        remainingShares = user.shares_FPA;
                        break;

                    case "DL24":

                        user.shares_DL24 -= _SoldAmount;
                        remainingShares = user.shares_DL24;
                        break;

                }

                if (remainingShares >= 0)
                {





                    try
                    {
                        user.StockOperations.Add(new Operation
                        {

                            Code = stock.Code,
                            CreatedAt = System.DateTime.UtcNow,
                            AmountPurchased = 0,
                            AmountSold = _SoldAmount,
                            Price = _Price,
                            Value = SellValue


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
                    TempData["errorInfo"] = "You don't have enough shares to complete this transaction!";
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
