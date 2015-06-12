using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FPtask.Models
{
 
    public class Shares
    {
        [Required]
        [Key]
        public int ShareId { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name="Amount Available")]
        public int AmountAvailable { get; set; }

        [Required]
        public DateTime LastUpdate { get; set; }


        public virtual ICollection<FPtask.Models.SharePriceHistory> PriceHistory { get; set; }

    }


    public class SharePriceHistory
    {

        [Key]
        public int HistoryId { get; set; }

        [Required]

        public DateTime PublicationDate { get; set; }


        [Required]
        public double Price { get; set; }

        [Required]
        public string UpdatersIp { get; set; }



    }

}