using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Contracts.V1.Requests.Queries
{
    public class GetAllExpensesQuery
    {
        [FromQuery(Name = "userId")]
        public string UserId { get; set; }
    }
}