using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ExpenseTracker.Contracts.V1;
using ExpenseTracker.IntegrationTest;
using FluentAssertions;
using ExpenseTracker.Contracts.V1;
using ExpenseTracker.Contracts.V1.Requests;
using ExpenseTracker.Contracts.V1.Responses;
using ExpenseTracker.Domain;
using Xunit;

namespace Tweetbook.IntegrationTests
{
    public class PostsControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetAll_WithoutAnyPosts_ReturnsEmptyResponse()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Expenses.GetAll);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            (await response.Content.ReadAsAsync<PagedResponse<Expense>>()).Data.Should().BeEmpty();
        }

        [Fact]
        public async Task Get_ReturnsPost_WhenPostExistsInTheDatabase()
        {
            // Arrange
            await AuthenticateAsync();
            var createdResponse = await CreateExpenseAsync(new CreateExpenseRequest
            {
                Title = "Test expense",
                Value = (float)2.0,
                Tags = new List<string>
                {
                    "Test tag",
                    "Test tag2"
                }
            });

            var createdExpense = (await createdResponse.Content.ReadAsAsync<Response<ExpenseResponse>>()).Data;

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Expenses.Get.Replace("{expenseId}", createdExpense.Id.ToString()));
            
            // Assert
            createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedExpense = (await response.Content.ReadAsAsync<Response<ExpenseResponse>>()).Data;
            returnedExpense.Id.Should().Be(createdExpense.Id);
            returnedExpense.Title.Should().Be("Test expense");
            returnedExpense.Value.Should().Be((float)2.0);
        }
    }
}