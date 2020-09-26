using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using ExpenseTracker.Contracts.V1;
using ExpenseTracker.Contracts.V1.Requests;
using ExpenseTracker.Contracts.V1.Responses;
using ExpenseTracker.Domain;
using ExpenseTracker.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Extensions;

namespace ExpenseTracker.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Poster")]
    [Produces("application/json")]
    public class TagsController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IMapper _mapper;
        
        public TagsController(IMapper mapper, ITagService tagService)
        {
            _mapper = mapper;
            _tagService = tagService;
        }
        
        /// <summary>
        /// Returns all the tags in the system
        /// </summary>
        /// <response code="200">Returns all the tags in the system</response>
        [HttpGet(ApiRoutes.Tags.GetAll)]
        // [Authorize(Policy = "TagViewer")] // Used for CLAIMS
        public async Task<IActionResult> GetAll()
        {
            return Ok(_mapper.Map<List<TagResponse>>(await _tagService.GetAllTagsAsync()));
        }
        
        /// <summary>
        /// Create tag in the system
        /// </summary>
        /// <response code="201">Return created tag</response>
        /// <response code="400">Unable to create the tag due to validation error</response>
        [HttpPost(ApiRoutes.Tags.Create)]
        [ProducesResponseType(typeof(TagResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest request)
        {
            if (!ModelState.IsValid)
            {
                
            }
            
            var tag = new Tag()
            {
                Name = request.Name,
                CreatedOn = DateTime.UtcNow,
                CreatorId = HttpContext.GetUserId()
            };

            return Ok(_mapper.Map<TagResponse>(await _tagService.CreateTagAsync(tag)));
        }
        
        [HttpDelete(ApiRoutes.Tags.Delete)]
        [Authorize(Roles = "Admin")]
        [Authorize(Policy = "WorksForMe")]
        public async Task<IActionResult> Delete([FromBody] CreateTagRequest request)
        {
            var tag = new Tag()
            {
                Name = request.Name
            };
            
            var deleted = await _tagService.DeleteTagAsync(tag);

            if (deleted)
                return NoContent();

            return NotFound();
        }
    }
}