using System.Collections.Generic;

namespace ExpenseTracker.AuthApi.Contracts.V1.Responses
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}