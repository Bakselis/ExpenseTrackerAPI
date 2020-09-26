using AutoMapper;
using ExpenseTracker.Contracts.V1.Requests.Queries;
using ExpenseTracker.Domain;

namespace ExpenseTracker.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
            CreateMap<GetAllExpensesQuery, GetAllExpensesFilter>();
        }
    }
}