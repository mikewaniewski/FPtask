using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FPtask.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;


         
        }

        [Required]
        [Range(0.0, Double.MaxValue)]
        [Display(Name="PLN Amount")]
        public double MoneyAvailable { get; set; }



        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "FP shares")]
        public int shares_FP { get; set; }


        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "FPL shares")]
        public int shares_FPL { get; set; }


        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "FPC shares")]
        public int shares_FPC { get; set; }


        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "PGB shares")]
        public int shares_PGB { get; set; }


        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "FPA shares")]
        public int shares_FPA { get; set; }


        [Required]
        [Range(0, int.MaxValue)]
        [Display(Name = "DL24 shares")]
        public int shares_DL24 { get; set; }



        
        public virtual ICollection<FPtask.Models.Operation> StockOperations { get; set; }
        


         
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        public System.Data.Entity.DbSet<FPtask.Models.Shares> StockShares { get; set; }
         


 
    }
}