using System.Collections.Generic;

namespace ExpenseTracker.Contracts.V1.Responses
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}