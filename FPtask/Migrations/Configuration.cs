namespace FPtask.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using FPtask.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<FPtask.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(FPtask.Models.ApplicationDbContext context)
        {


            context.StockShares.AddOrUpdate(

                new Shares { ShareId = 1, Code = "FP", Name = "Future Processing", AmountAvailable = 1000, LastUpdate = System.DateTime.Now },

                new Shares { ShareId = 2, Code = "FPL", Name = "FP Lab", AmountAvailable = 1000, LastUpdate = System.DateTime.Now },

                new Shares { ShareId = 3, Code = "FPC", Name = "FP Coin", AmountAvailable = 1000, LastUpdate = System.DateTime.Now },

                new Shares { ShareId = 4, Code = "PGB", Name = "ProgressBar", AmountAvailable = 1000, LastUpdate = System.DateTime.Now },

                new Shares { ShareId = 5, Code = "FPA", Name = "FP Adventure", AmountAvailable = 1000, LastUpdate = System.DateTime.Now },

                new Shares { ShareId = 6, Code = "DL24", Name = "DeadLine24", AmountAvailable = 1000, LastUpdate = System.DateTime.Now }


                );


         
        }
    }
}
