//using System.Linq;
//using System.Security.Claims;
//using AutoMapper;
//using ExpenseTracker.ExpenseTracker.Contracts2.V1.Requests;
//using ExpenseTracker.Domain;
//using Microsoft.AspNetCore.Http;
//
//namespace ExpenseTracker.MappingProfiles.Resolvers
//{
//    public class AssignUserIdToTagFromCreateTagRequest : IValueResolver<CreateTagRequest, Tag, string>
//    {
//        private IHttpContextAccessor _httpContextAccessor;
//
//        public AssignUserIdToTagFromCreateTagRequest(IHttpContextAccessor httpContextAccessor) 
//        {
//            _httpContextAccessor = httpContextAccessor;
//        }
//        
//        public string Resolve(CreateTagRequest source, Tag destination, string destMember, ResolutionContext context)
//        {
//            return Helper.GetClaimsFromUser(_httpContextAccessor.HttpContext?.User.Identity as ClaimsIdentity);
//        }
//    }
//}