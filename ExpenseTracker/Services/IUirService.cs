using System;
using ExpenseTracker.Contracts.V1.Requests.Queries;

namespace ExpenseTracker.Services
{
    public interface IUirService
    {
        Uri GetPostUri(string expenseID);
        Uri GetAllPostUri(PaginationQuery paginationQuery = null);
    }
}