using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;
using ExpenseTracker.Contracts.V1;
using ExpenseTracker.Contracts.V1.Requests;
using ExpenseTracker.Contracts.V1.Responses;
using ExpenseTracker.Data;

namespace ExpenseTracker.IntegrationTest
{
    public class IntegrationTest : IDisposable
    {
        protected readonly HttpClient TestClient;
        private readonly IServiceProvider _serviceProvider;
        protected IntegrationTest()
        {
            var appFactory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder => {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DataContext));
                        services.AddDbContext<DataContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });
                    });
                });
            _serviceProvider = appFactory.Services;
            TestClient = appFactory.CreateClient();
        }

        protected async Task<HttpResponseMessage> CreateExpenseAsync(CreateExpenseRequest request)
        {
            return await TestClient.PostAsJsonAsync(ApiRoutes.Expenses.Create, request);
        }
        
        protected async Task<HttpResponseMessage> UpdateExpenseAsync(Guid expenseId, UpdateExpenseRequest request)
        {
            var t = ApiRoutes.Expenses.Update.Replace("{expenseId}", expenseId.ToString());
            return await TestClient.PutAsJsonAsync(ApiRoutes.Expenses.Update.Replace("{expenseId}", expenseId.ToString()), request);
        }

        protected async Task AuthenticateAsync()
        {
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await GetJwtAsync());
        }

        private async Task<string> GetJwtAsync()
        {
            var response = await TestClient.PostAsJsonAsync(ApiRoutes.Identity.Register, new UserRegistrationRequest
            {
                Email="test@integrationtests.com",
                Password = "SomePassword123!"
            });
            
            var registrationResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
            return registrationResponse.Token;
        }

        public void Dispose()
        {
            using var servicesScrope = _serviceProvider.CreateScope();
            var context = servicesScrope.ServiceProvider.GetService<DataContext>();
            context.Database.EnsureDeleted();
        }
    }
}