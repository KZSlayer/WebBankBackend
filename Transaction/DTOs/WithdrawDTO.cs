﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Transaction.DTOs
{
    public class WithdrawDTO
    {

        [Required]
        public decimal Amount { get; set; }
    }
}
