using ExpenseTracker.Contracts.V1.Requests;
using Swashbuckle.AspNetCore.Filters;

namespace ExpenseTracker.SwaggerExamples.Requests
{
    public class CreateTagRequestExample : IExamplesProvider<CreateTagRequest>
    {
        public CreateTagRequest GetExamples()
        {
            return new CreateTagRequest
            {
                Name = "New tag name"
            };
        }
    }
}