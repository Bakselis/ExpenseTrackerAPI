using System.Linq;
using System.Security.Cryptography;
using AutoMapper;
using ExpenseTracker.Contracts.V1.Responses;
using ExpenseTracker.Domain;

namespace ExpenseTracker.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Expense, ExpenseResponse>()
                .ForMember(dest => dest.Tags, opt =>
                    opt.MapFrom(src => src.Tags.Select(x => new TagResponse {Name = x.TagName}
                        )
                    )
                );
            
            CreateMap<Tag, TagResponse>();
        }
    }
}