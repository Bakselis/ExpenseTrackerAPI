using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Domain
{
    public class ExpenseTag
    {
        [ForeignKey(nameof(TagName))]
        public virtual Tag Tag { get; set; }

        public string TagName { get; set; }
        
        public virtual Expense Expense { get; set; }

        public Guid ExpenseId { get; set; }
    }
}