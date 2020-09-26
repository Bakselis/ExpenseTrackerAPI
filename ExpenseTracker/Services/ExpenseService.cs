using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExpenseTracker.Data;
using ExpenseTracker.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly DataContext _dataContext;

        public ExpenseService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<List<Expense>> GetExpensesAsync(GetAllExpensesFilter expensesFilter = null, PaginationFilter paginationFilter = null, string userId = null)
        {
            var queryable = _dataContext.Expenses.AsQueryable();

            queryable = AddFiltersOnQueryable(expensesFilter, queryable);
            
            if (paginationFilter == null)
            {
                return await queryable.Include(x => x.Tags).ToListAsync();
            }
            
            if (userId != null)
            {
                queryable = queryable.Where(x => x.UserId == userId);
            }
            
            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            
            return await queryable.Include(x => x.Tags).Skip(skip).Take(paginationFilter.PageSize).ToListAsync();
        }

        public async Task<Expense> GetExpenseByIdAsync(Guid expenseId)
        {
            return await _dataContext.Expenses
                .Include(x => x.Tags)
                .SingleOrDefaultAsync(x => x.Id == expenseId);
        }

        public async Task<bool> CreateExpenseAsync(Expense expense)
        {
            expense.Tags?.ForEach(x=>x.TagName = x.TagName.ToLower());
            
            await AddNewTags(expense);
            await _dataContext.Expenses.AddAsync(expense);

            var created = await _dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> UpdateExpenseAsync(Expense expenseToUpdate)
        {
            expenseToUpdate.Tags?.ForEach(x=>x.TagName = x.TagName.ToLower());
            await AddNewTags(expenseToUpdate);
            _dataContext.Expenses.Update(expenseToUpdate);
            var updated = await _dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeleteExpenseAsync(Guid postId)
        {
            var expense = await GetExpenseByIdAsync(postId);

            if (expense == null)
                return false;

            _dataContext.Expenses.Remove(expense);
            var deleted = await _dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> UserOwnsExpenseAsync(Guid expenseId, string userId)
        {
            var expense = await _dataContext.Expenses.AsNoTracking().SingleOrDefaultAsync(x => x.Id == expenseId);

            if (expense == null)
            {
                return false;
            }

            if (expense.UserId != userId)
            {
                return false;
            }

            return true;
        }

        private async Task AddNewTags(Expense expense)
        {
            foreach (var tag in expense.Tags)
            {
                var existingTag =
                    await _dataContext.Tags.SingleOrDefaultAsync(x =>
                        x.Name == tag.TagName);
                if (existingTag != null)
                    continue;

                await _dataContext.Tags.AddAsync(new Tag
                    {Name = tag.TagName, CreatedOn = DateTime.UtcNow, CreatorId = expense.UserId});
            }
        }
        
        private static IQueryable<Expense> AddFiltersOnQueryable(GetAllExpensesFilter expensesFilter, IQueryable<Expense> queryable)
        {
            if (!string.IsNullOrEmpty(expensesFilter?.UserId))
            {
                queryable = queryable.Where(x => x.UserId == expensesFilter.UserId);
            }

            return queryable;
        }
    }
}