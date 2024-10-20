using Rido.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rido.Common.Models.Requests
{
    
        public class WalletTransactionRequestDto
        {
       

            [Required]
            [Range(100, double.MaxValue, ErrorMessage = "Amount must be greater than 100")]
            public decimal Amount { get; set; }



        }
    }


