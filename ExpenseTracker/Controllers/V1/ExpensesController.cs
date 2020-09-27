using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.Cache;
using ExpenseTracker.Contracts.V1;
using ExpenseTracker.Contracts.V1.Requests;
using ExpenseTracker.Contracts.V1.Requests.Queries;
using ExpenseTracker.Contracts.V1.Responses;
using ExpenseTracker.Domain;
using ExpenseTracker.Helpers;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Extensions;

namespace ExpenseTracker.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ExpensesController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly IMapper _mapper;
        private readonly IUirService _uriService;

        public ExpensesController(IExpenseService expenseService, IMapper mapper, IUirService uirService)
        {
            _expenseService = expenseService;
            _mapper = mapper;
            _uriService = uirService;
        }

        [HttpGet(ApiRoutes.Expenses.GetAll)]
        [Cached(600)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllExpensesQuery getAllExpensesQuery, [FromQuery] PaginationQuery paginationQuery)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationQuery);
            var filter = _mapper.Map<GetAllExpensesFilter>(getAllExpensesQuery);
            var expenses = await _expenseService.GetExpensesAsync(filter, paginationFilter, HttpContext.GetUserId());
            
            var expensesResponse = _mapper.Map<List<ExpenseResponse>>(expenses);

            if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
            {
                return Ok(new PagedResponse<ExpenseResponse>(expensesResponse));
            }

            var paginationResponse = PaginationHelpers<ExpenseResponse>.CreatePaginationResponse(_uriService, paginationFilter, expensesResponse);
            
            return Ok(paginationResponse);
        }

        [HttpPut(ApiRoutes.Expenses.Update)]
        public async Task<IActionResult> Update([FromRoute]Guid expenseId, [FromBody] UpdateExpenseRequest request)
        {
            var userOwnsExpenses = await _expenseService.UserOwnsExpenseAsync(expenseId, HttpContext.GetUserId());

            if (!userOwnsExpenses)
            {
                return BadRequest(new {error = "You do not own this expense"});
            }

            var expense = await _expenseService.GetExpenseByIdAsync(expenseId);

            expense.Title = request.Title;
            expense.Value = request.Value;
            expense.DateAdded = DateTime.Now;
            expense.Tags = request.Tags.Select(x => new ExpenseTag {ExpenseId = expenseId, TagName = x})
                .ToList();

            var updated = await _expenseService.UpdateExpenseAsync(expense);

            if(updated)
                return Ok(new Response<ExpenseResponse>(_mapper.Map<ExpenseResponse>(expense)));

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Expenses.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid expenseId)
        {
            var userOwnsPost = await _expenseService.UserOwnsExpenseAsync(expenseId, HttpContext.GetUserId());

            if (!userOwnsPost)
            {
                return BadRequest(new {error = "You do not own this post"});
            }
            
            var deleted = await _expenseService.DeleteExpenseAsync(expenseId);

            if (deleted)
                return NoContent();

            return NotFound();
        }

        [HttpGet(ApiRoutes.Expenses.Get)]
        public async Task<IActionResult> Get([FromRoute]Guid expenseId)
        {
            var expense = await _expenseService.GetExpenseByIdAsync(expenseId);

            if (expense == null)
                return NotFound();

            return Ok(new Response<ExpenseResponse>(_mapper.Map<ExpenseResponse>(expense)));
        }

        [HttpPost(ApiRoutes.Expenses.Create)]
        public async Task<IActionResult> Create([FromBody] CreateExpenseRequest expenseRequest)
        {
            var newExpenseId = Guid.NewGuid();
            var expense = new Expense
            {
                Id = newExpenseId,
                Title = expenseRequest.Title,
                UserId = HttpContext.GetUserId(),
                Value = expenseRequest.Value,
                DateAdded = DateTime.Now,
                Tags = expenseRequest.Tags.Select(x=> new ExpenseTag{ExpenseId = newExpenseId, TagName = x}).ToList()
            };
            
            await _expenseService.CreateExpenseAsync(expense);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Expenses.Get.Replace("{newExpenseId}", expense.Id.ToString());

            return Created(locationUri, new Response<ExpenseResponse>(_mapper.Map<ExpenseResponse>(expense)));
        }
    }
}