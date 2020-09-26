using ExpenseTracker.Contracts.V1.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace ExpenseTracker.SwaggerExamples.Response
{
    public class TagResponseExample : IExamplesProvider<TagResponse>
    {
        public TagResponse GetExamples()
        {
            return new TagResponse
            {
                Name = "New tag name"
            };
        }
    }
}