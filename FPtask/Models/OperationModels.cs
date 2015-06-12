using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FPtask.Models
{



    public class Operation
    {
        [Required]
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }


        [Required]
        public int AmountPurchased { get; set; }


        [Required]
        public int AmountSold { get; set; }


        [Required]
        public double Price { get; set; }


        [Required]
        public double Value { get; set; }
         

    }


 





    public class SellViewModel
    {

        public int ShareId { get; set; }
        
        public string Code { get; set; }
        public string Name { get; set; }
        public int AmountAvailableForUser { get; set; }

    }

}