using System.Collections.Generic;

namespace ExpenseTracker.Contracts.V1.Requests
{
    public class CreateExpenseRequest
    {
        public string Title { get; set; }
        
        public float Value { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}