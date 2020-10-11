using System.Linq;
using AutoMapper;
using ExpenseTracker.Contracts.V1.Requests;
using ExpenseTracker.Contracts.V1.Requests.Queries;
using ExpenseTracker.Contracts.V1.Responses;
using ExpenseTracker.Domain;

namespace ExpenseTracker.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<PaginationQuery, PaginationFilter>();
            CreateMap<GetAllExpensesQuery, GetAllExpensesFilter>();
//            CreateMap<CreateTagRequest, Tag>()
//                .ForMember(dest => dest.CreatedOn, opt => opt.MapFrom<CustomResolver>());
        }
    }
}