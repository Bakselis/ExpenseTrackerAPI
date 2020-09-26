using System.Collections.Generic;
using System.Linq;
using ExpenseTracker.Contracts.V1.Requests.Queries;
using ExpenseTracker.Contracts.V1.Responses;
using ExpenseTracker.Domain;
using ExpenseTracker.Services;
using Microsoft.EntityFrameworkCore.Internal;

namespace ExpenseTracker.Helpers
{
    public class PaginationHelpers<T>
    {
        public static PagedResponse<T> CreatePaginationResponse(IUirService uriService, PaginationFilter paginationFilter, List<T> response)
        {
            var nextPage = paginationFilter.PageNumber >= 1
                ? uriService.GetAllPostUri(new PaginationQuery(paginationFilter.PageNumber + 1,
                    paginationFilter.PageSize)).ToString()
                : null; 
            
            var previuosPage = paginationFilter.PageNumber - 1 >= 1
                ? uriService.GetAllPostUri(new PaginationQuery(paginationFilter.PageNumber - 1,
                    paginationFilter.PageSize)).ToString()
                : null; 
            
            return new PagedResponse<T>
            {
                Data = response,
                PageNumber = paginationFilter.PageNumber >= 1 ? paginationFilter.PageNumber : (int?)null,
                PageSize  = paginationFilter.PageSize >= 1 ? paginationFilter.PageSize : (int?)null,
                NextPage = response.Any() ? nextPage : null,
                PreviuosPage = previuosPage
            };
        }
    }
}