using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MzadPalestine.Core.DTOs.Categories;
using MzadPalestine.Core.Interfaces.Repositories;
using MzadPalestine.Core.Interfaces.Services;
using MzadPalestine.Core.Models;
using MzadPalestine.Core.Models.Common;

namespace MzadPalestine.API.Controllers;

public class CategoriesController : BaseApiController
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly ICacheService _cacheService;
    private readonly ICurrentUserService _currentUserService;

    public CategoriesController(
        ICategoryRepository categoryRepository,
        IFileStorageService fileStorageService,
        ICacheService cacheService,
        ICurrentUserService currentUserService)
    {
        _categoryRepository = categoryRepository;
        _fileStorageService = fileStorageService;
        _cacheService = cacheService;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var cacheKey = "categories_all";
        var cachedResult = await _cacheService.GetAsync<IEnumerable<CategoryDto>>(cacheKey);

        if (cachedResult != null)
        {
            return Ok(cachedResult);
        }

        var categories = await _categoryRepository.GetAllAsync();

        var result = categories.Select(c => new CategoryDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            ImageUrl = c.ImageUrl,
            ParentId = c.ParentId,
            DisplayOrder = c.DisplayOrder,
            ListingCount = c.Listings.Count
        }).ToList();

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromHours(1));

        return Ok(result);
    }

    [HttpGet("tree")]
    public async Task<ActionResult<IEnumerable<CategoryTreeDto>>> GetCategoryTree()
    {
        var cacheKey = "categories_tree";
        var cachedResult = await _cacheService.GetAsync<IEnumerable<CategoryTreeDto>>(cacheKey);

        if (cachedResult != null)
        {
            return Ok(cachedResult);
        }

        var categories = await _categoryRepository.GetAllAsync();
        var rootCategories = categories.Where(c => !c.ParentId.HasValue)
            .OrderBy(c => c.DisplayOrder);

        var result = rootCategories.Select(c => BuildCategoryTree(c, categories)).ToList();

        await _cacheService.SetAsync(cacheKey, result, TimeSpan.FromHours(1));

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CategoryDetailsDto>> GetCategory(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        return new CategoryDetailsDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ImageUrl = category.ImageUrl,
            ParentId = category.ParentId,
            ParentName = category.Parent?.Name,
            DisplayOrder = category.DisplayOrder,
            ListingCount = category.Listings.Count,
            Children = category.Children.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                ParentId = c.ParentId,
                DisplayOrder = c.DisplayOrder,
                ListingCount = c.Listings.Count
            }).ToList()
        };
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> CreateCategory([FromForm] CreateCategoryRequest request)
    {
        var category = new Category
        {
            Name = request.Name,
            Description = request.Description,
            ParentId = request.ParentId,
            DisplayOrder = request.DisplayOrder,
            CreatedAt = DateTime.UtcNow
        };

        if (request.Image != null)
        {
            category.ImageUrl = await _fileStorageService.SaveFileAsync(request.Image, "categories");
        }

        await _categoryRepository.AddAsync(category);

        // Invalidate cache
        await _cacheService.RemoveAsync("categories_all");
        await _cacheService.RemoveAsync("categories_tree");

        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ImageUrl = category.ImageUrl,
            ParentId = category.ParentId,
            DisplayOrder = category.DisplayOrder
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<CategoryDto>> UpdateCategory(int id, [FromForm] UpdateCategoryRequest request)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        category.Name = request.Name;
        category.Description = request.Description;
        category.ParentId = request.ParentId;
        category.DisplayOrder = request.DisplayOrder;

        if (request.Image != null)
        {
            if (!string.IsNullOrEmpty(category.ImageUrl))
            {
                await _fileStorageService.DeleteFileAsync(category.ImageUrl);
            }
            category.ImageUrl = await _fileStorageService.SaveFileAsync(request.Image, "categories");
        }

        await _categoryRepository.UpdateAsync(category);

        // Invalidate cache
        await _cacheService.RemoveAsync("categories_all");
        await _cacheService.RemoveAsync("categories_tree");

        return new CategoryDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ImageUrl = category.ImageUrl,
            ParentId = category.ParentId,
            DisplayOrder = category.DisplayOrder
        };
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _categoryRepository.GetByIdAsync(id);

        if (category == null)
        {
            return NotFound();
        }

        if (category.Listings.Any())
        {
            return BadRequest("لا يمكن حذف التصنيف لوجود مزادات مرتبطة به");
        }

        if (category.Children.Any())
        {
            return BadRequest("لا يمكن حذف التصنيف لوجود تصنيفات فرعية");
        }

        if (!string.IsNullOrEmpty(category.ImageUrl))
        {
            await _fileStorageService.DeleteFileAsync(category.ImageUrl);
        }

        await _categoryRepository.DeleteAsync(category);

        // Invalidate cache
        await _cacheService.RemoveAsync("categories_all");
        await _cacheService.RemoveAsync("categories_tree");

        return NoContent();
    }

    private CategoryTreeDto BuildCategoryTree(Category category, IEnumerable<Category> allCategories)
    {
        var children = allCategories
            .Where(c => c.ParentId == category.Id)
            .OrderBy(c => c.DisplayOrder)
            .Select(c => BuildCategoryTree(c, allCategories))
            .ToList();

        return new CategoryTreeDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            ImageUrl = category.ImageUrl,
            DisplayOrder = category.DisplayOrder,
            ListingCount = category.Listings.Count,
            Children = children
        };
    }
}
