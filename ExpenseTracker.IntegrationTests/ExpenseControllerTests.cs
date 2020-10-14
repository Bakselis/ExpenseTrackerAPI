using System;
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

namespace ExpenseTracker.IntegrationTests
{
    public class ExpenseControllerTests : IntegrationTest.IntegrationTest
    {
        [Fact]
        public async Task GetAll_WithoutAnyExpenses_ReturnsEmptyResponse()
        {
            // Arrange
            Authenticate();

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Expenses.GetAll);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            
            (await response.Content.ReadAsAsync<PagedResponse<Expense>>()).Data.Should().BeEmpty();
        }
        
        [Fact]
        public async Task GetExpense_WithoutAnyExpenses_ReturnsEmptyResponse()
        {
            // Arrange
            Authenticate();

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Expenses.Get.Replace("{expenseId}", new Guid().ToString()));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_ReturnsExpense_WhenExpenseIsCreated()
        {
            // Arrange
            Authenticate();
            
            // Act
            var createdResponse = await CreateTemplateExpense();

            var createdExpense = (await createdResponse.Content.ReadAsAsync<Response<ExpenseResponse>>()).Data;
            
            // Assert
            createdResponse.StatusCode.Should().Be(HttpStatusCode.Created);
            createdExpense.Id.Should().Be(createdExpense.Id);
            createdExpense.Title.Should().Be("Test expense");
            createdExpense.Value.Should().Be((float)2.0);
        }
        
        [Fact]
        public async Task Get_ReturnsExpense_WhenExpenseExistsInTheDatabase()
        {
            // Arrange
            Authenticate();
            var createdResponse = await CreateTemplateExpense();

            var createdExpense = (await createdResponse.Content.ReadAsAsync<Response<ExpenseResponse>>()).Data;

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Expenses.Get.Replace("{expenseId}", createdExpense.Id.ToString()));
            
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedExpense = (await response.Content.ReadAsAsync<Response<ExpenseResponse>>()).Data;
            returnedExpense.Id.Should().Be(createdExpense.Id);
            returnedExpense.Title.Should().Be("Test expense");
            returnedExpense.Value.Should().Be((float)2.0);
        }
        
        [Fact]
        public async Task Put_ReturnsOkAndUpdatedExpenseInTheDatabase_WhenExpenseIsUpdated()
        {
            // Arrange
            Authenticate();
            var createdResponse = (await (await CreateTemplateExpense()).Content.ReadAsAsync<Response<ExpenseResponse>>()).Data;
            
            var updatedResponse = await UpdateExpenseAsync(createdResponse.Id, new UpdateExpenseRequest
            {
                Title = "Updated expense",
                Value = (float)10.0,
                Tags = new List<string>
                {
                    "Updated Tag",
                }
            });
            
            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Expenses.Get.Replace("{expenseId}", createdResponse.Id.ToString()));
            
            // Assert
            updatedResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedExpense = (await response.Content.ReadAsAsync<Response<ExpenseResponse>>()).Data;
            returnedExpense.Id.Should().Be(createdResponse.Id);
            returnedExpense.Title.Should().Be("Updated expense");
            returnedExpense.Value.Should().Be((float)10.0);
        }
        
        private async Task<HttpResponseMessage> CreateTemplateExpense()
        {
            var createdResponse = await CreateExpenseAsync(new CreateExpenseRequest
            {
                Title = "Test expense",
                Value = (float) 2.0,
                Tags = new List<string>
                {
                    "Test tag",
                    "Test tag2"
                }
            });
            return createdResponse;
        }
    }
}