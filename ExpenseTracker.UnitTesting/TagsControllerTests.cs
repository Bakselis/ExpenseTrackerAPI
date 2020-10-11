using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;
using AutoMapper;
using ExpenseTracker.Contracts.V1.Requests;
using ExpenseTracker.Contracts.V1.Responses;
using ExpenseTracker.Controllers.V1;
using ExpenseTracker.Domain;
using ExpenseTracker.Services;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.UnitTesting
{
    public class TagsControllerTests
    {
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfBrainstormSessions()
        {
            // Arrange
            var mockTagRepo = new Mock<ITagService>();
            mockTagRepo.Setup(repo => repo.GetAllTagsAsync())
                .ReturnsAsync(GetTagsTest());

            var autoMapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Tag,TagResponse>();
                }
            );

            var mapper = autoMapperConfiguration.CreateMapper();
            
            var controller = new TagsController(mapper, mockTagRepo.Object);

            // Act
            var result = await controller.GetAll();

            // Assert
            var objectResult = Assert.IsType<OkObjectResult>(result);
            var model = Assert.IsAssignableFrom<List<TagResponse>>(objectResult.Value);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task CreateTag_ReturnsCreatedResult_WhenTagIsNotInTheSystem()
        {
            // Arrange
            var mockTagRepo = new Mock<ITagService>();
            var newUserId = new Guid();

            var tag = new Tag
            {
                Name = "tag",
                CreatorId = newUserId.ToString(),
                CreatedOn = DateTime.Now
            };
            
            mockTagRepo.Setup(repo => repo.CreateTagAsync(It.IsAny<Tag>()))
                .ReturnsAsync(true);

            var autoMapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Tag,CreateTagRequest>();
                    cfg.CreateMap<Tag,TagResponse>();
                }
            );
            var mapper = autoMapperConfiguration.CreateMapper();
            
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("id", newUserId.ToString())
            }, "mock"));

            var tagController = new TagsController(mapper, mockTagRepo.Object);
            tagController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = await tagController.Create(mapper.Map<CreateTagRequest>(tag));

            // Assert
            var resultCraeted = result.Should().BeCreatedResult();
            resultCraeted.Value.Should().BeEquivalentTo(mapper.Map<TagResponse>(tag));
            resultCraeted.Location.Should().NotBeEmpty();
        }
        
        [Fact]
        public async Task CreateTag_ReturnsBadRequest_WhenTagIsInTheSystem()
        {
            // Arrange
            var mockTagRepo = new Mock<ITagService>();
            var newUserId = new Guid();

            var tag = new Tag
            {
                Name = "tag",
                CreatorId = newUserId.ToString(),
                CreatedOn = DateTime.Now
            };
            
            mockTagRepo.Setup(repo => repo.CreateTagAsync(It.IsAny<Tag>()))
                .ReturnsAsync(false);

            var autoMapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Tag,CreateTagRequest>();
                    cfg.CreateMap<Tag,TagResponse>();
                }
            );
            var mapper = autoMapperConfiguration.CreateMapper();
            
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim("id", newUserId.ToString())
            }, "mock"));

            var tagController = new TagsController(mapper, mockTagRepo.Object);
            tagController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            var result = await tagController.Create(mapper.Map<CreateTagRequest>(tag));

            // Assert
            result.Should().BeBadRequestObjectResult();
        }
        
        [Fact]
        public async Task DeleteTag_ReturnsNoContent_WhenTagInInTheSystem()
        {
            // Arrange
            var mockTagRepo = new Mock<ITagService>();
            mockTagRepo.Setup(repo => repo.DeleteTagAsync("tag"))
                .ReturnsAsync(true);

            var autoMapperConfiguration = new MapperConfiguration(cfg =>{});
            var mapper = autoMapperConfiguration.CreateMapper();
            
            var tagController = new TagsController(mapper, mockTagRepo.Object);

            // Act
            var result = await tagController.Delete("tag");

            // Assert
            result.Should().BeNoContentResult();
        }
        
        [Fact]
        public async Task DeleteTag_ReturnsNotFound_WhenTagIsNotInTheSystem()
        {
            // Arrange
            var mockTagRepo = new Mock<ITagService>();
            mockTagRepo.Setup(repo => repo.DeleteTagAsync("tag"))
                .ReturnsAsync(false);

            var autoMapperConfiguration = new MapperConfiguration(cfg =>{});
            var mapper = autoMapperConfiguration.CreateMapper();
            
            var tagController = new TagsController(mapper, mockTagRepo.Object);

            // Act
            var result = await tagController.Delete("tag");

            // Assert
            result.Should().BeNotFoundResult();
        }
        
        private List<Tag> GetTagsTest()
        {
            var tags = new List<Tag>();
            tags.Add(new Tag()
            {
                Name = "tag",
                CreatorId = "1234",
                CreatedOn = DateTime.Now,
            });
            tags.Add(new Tag()
            {
                Name = "tag",
                CreatorId = "1234",
                CreatedOn = DateTime.Now,
            });
            return tags;
        }
    }
}