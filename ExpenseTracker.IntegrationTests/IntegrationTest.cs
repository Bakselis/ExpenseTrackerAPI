using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using ExpenseTracker.AuthApi.Contracts.V1.Responses;
using ExpenseTracker.Cache;
using ExpenseTracker.Contracts.V1;
using ExpenseTracker.Contracts.V1.Requests;
using ExpenseTracker.Data;
using IntegrationTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace ExpenseTracker.IntegrationTest
{
    public class IntegrationTest : IDisposable
    {
        protected readonly HttpClient TestClient;
        private readonly IServiceProvider _serviceProvider;
        
        public static ILogger<ConsoleLoggerProvider> AppLogger = null;
        public static ILoggerFactory loggerFactory = null;
        
        protected IntegrationTest()
        {
            var projectDir = Directory.GetCurrentDirectory();
            var configPath = Path.Combine(projectDir, "appsettings.json");
            var appFactory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder => {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DataContext));
                        services.AddDbContext<DataContext>(options =>
                        {
                            options.UseInMemoryDatabase("TestDb");
                        });
                        services.AddLogging(build => build.AddConsole().AddFilter(level => level >= LogLevel.Trace));
                        loggerFactory = services.BuildServiceProvider().GetService<ILoggerFactory>();
                        AppLogger = loggerFactory.CreateLogger<ConsoleLoggerProvider>();
                    });
                    builder.ConfigureAppConfiguration((context, conf) =>
                    {
                        conf.AddJsonFile(configPath);
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

        protected void Authenticate()
        {
            var claims = MockJwtTokens.UserClaims();
            var jwt = MockJwtTokens.GenerateJwtToken(claims);
            TestClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", jwt);
        }

//        private async Task<string> GetJwtAsync()
//        {
//            var claims = await MockJwtTokens.UserClaims();
//            return MockJwtTokens.GenerateJwtToken(claims);
//            var response = await TestClientAuth.PostAsJsonAsync(AuthApi.Contracts.V1.ApiRoutes.Identity.Register, new UserRegistrationRequest
//            {
//                Email="test@integrationtests.com",
//                Password = "SomePassword123!"
//            });
//            
//            var registrationResponse = await response.Content.ReadAsAsync<AuthSuccessResponse>();
//            return registrationResponse.Token;
//        }

        public void Dispose()
        {
            using var servicesScrope = _serviceProvider.CreateScope();
            var context = servicesScrope.ServiceProvider.GetService<DataContext>();
            context.Database.EnsureDeleted();
        }
    }
}