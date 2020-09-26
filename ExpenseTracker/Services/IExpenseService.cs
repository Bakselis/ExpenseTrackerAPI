using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExpenseTracker.Domain;

namespace ExpenseTracker.Services
{
    public interface IExpenseService
    {
        Task<List<Expense>> GetExpensesAsync(GetAllExpensesFilter expensesFilter, PaginationFilter paginationFilter = null, string userId = null);

        Task<bool> CreateExpenseAsync(Expense expense);

        Task<Expense> GetExpenseByIdAsync(Guid expenseId);

        Task<bool> UpdateExpenseAsync(Expense expenseToUpdate);

        Task<bool> DeleteExpenseAsync(Guid postId);
        
        Task<bool> UserOwnsExpenseAsync(Guid expenseId, string userId);
    }
}