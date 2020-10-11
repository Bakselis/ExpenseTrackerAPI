using System;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Contracts.V1.Requests.Queries
{
    public class GetAllExpensesQuery
    {
        [FromQuery(Name = "value")]
        public float Value { get; set; }
    }
}