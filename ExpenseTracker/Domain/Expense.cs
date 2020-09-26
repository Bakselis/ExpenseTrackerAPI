using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Domain
{
    public class Expense
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string UserId { get; set; }
        public float Value { get; set; }

        public DateTime DateAdded { get; set; }

        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }

        public virtual List<ExpenseTag> Tags { get; set; }
    }
}