using System;
using ExpenseTracker.Contracts.V1;
using ExpenseTracker.Contracts.V1.Requests.Queries;
using Microsoft.AspNetCore.WebUtilities;

namespace ExpenseTracker.Services
{
    public class UIRService : IUirService
    {
    private readonly string _baseUri;

        public UIRService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetPostUri(string expenseID)
        {
            return new Uri(_baseUri + ApiRoutes.Expenses.Get.Replace("{expenseId}", expenseID));
        }

        public Uri GetAllPostUri(PaginationQuery paginationQuery = null)
        {
            var uri = new Uri(_baseUri + ApiRoutes.Expenses.GetAll);

            if (paginationQuery == null)
            {
                return uri;
            }

            var modifiedUri =
                QueryHelpers.AddQueryString(uri.ToString(), "PageNumber", paginationQuery.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "PageSize", paginationQuery.PageSize.ToString());
            
            return new Uri(modifiedUri);
        }
    }
}