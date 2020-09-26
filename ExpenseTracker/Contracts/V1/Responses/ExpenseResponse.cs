using System;
using System.Collections.Generic;

namespace ExpenseTracker.Contracts.V1.Responses
{
    public class ExpenseResponse
    {
        public Guid Id { get; set; }
        
        public string Title { get; set; }
        
        public float Value { get; set; }
        
        public DateTime DateAdded { get; set; }

        public string UserId { get; set; }

        public IEnumerable<TagResponse> Tags { get; set; }
    }
}