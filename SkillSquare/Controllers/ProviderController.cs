using DataAccessLayer.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace SkillSquare.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderRepository _providerRepository;

        public ProviderController(IProviderRepository providerRepository)
        {
            _providerRepository = providerRepository;
        }

        [HttpGet("by-category/{categoryId}")]
        public async Task<IActionResult> GetProvidersByCategory(int categoryId)
        {
            var providers = await _providerRepository.GetProvidersByCategoryAsync(categoryId);

            if (!providers.Any())
                return NotFound("No providers found for this category");

            return Ok(providers);
        }

        [HttpPost("registerServiceProvider")]
        public async Task<IActionResult> Register(ProviderRequest request)
        {
            var provider = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Area = request.Area,
                City = request.City,
                CategoryId = request.CategoryId,
                Skills = request.Skills,
                Bio = request.Bio,
                Role = "Provider",
                EmailConfirmed = true
            };

            var success = await _providerRepository.RegisterProviderAsync(provider, request.Password);
            if (!success) return BadRequest("Error registering provider");

            return Ok("Provider registered successfully");
        }

        // Get All Providers
        [HttpGet("GetAllServiceProviders")]
        public async Task<IActionResult> GetAll()
        {
            var providers = await _providerRepository.GetAllProvidersAsync();
            return Ok(providers);
        }

        // Get Provider by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var provider = await _providerRepository.GetProviderByIdAsync(id);
            if (provider == null) return NotFound();

            return Ok(provider);
        }

        [HttpGet("featured")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeaturedProviders([FromServices] IReviewRepository reviewRepo)
        {
            var providers = await _providerRepository.GetAllProvidersAsync();

            var featuredProviders = new List<object>();

            foreach (var p in providers.Where(p => p.Role == "Provider").Take(3))
            {
                var avgRating = await reviewRepo.GetAverageRatingForProviderAsync(p.Id);
                featuredProviders.Add(new
                {
                    ProviderId = p.Id,
                    Name = p.FirstName + " " + p.LastName,
                    CategoryName = p.Category != null ? p.Category.Title : "N/A",
                    HourlyRate = p.HourlyRate,
                    City = p.City,
                    Skills = p.Skills,
                    Bio = p.Bio,
                    AverageRating = avgRating
                });
            }

            return Ok(featuredProviders);
        }



        // Update Provider Profile
        [HttpPut("UpdateProfile/{id}")]
        [Authorize(Roles = "Admin,Provider")]
        public async Task<IActionResult> Update(string id, ProviderRequest request)
        {
            var provider = await _providerRepository.GetProviderByIdAsync(id);
            if (provider == null) return NotFound();

            provider.FirstName = request.FirstName;
            provider.LastName = request.LastName;
            provider.PhoneNumber = request.PhoneNumber;
            provider.Address = request.Address;
            provider.Area = request.Area;
            provider.City = request.City;
            provider.CategoryId = request.CategoryId;
            provider.Skills = request.Skills;
            provider.Bio = request.Bio;
            provider.HourlyRate = request.HourlyRate;

            var success = await _providerRepository.UpdateProviderAsync(provider);
            if (!success) return BadRequest("Error updating provider");

            return Ok("Provider updated successfully");
        }

        // Delete Provider
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _providerRepository.DeleteProviderAsync(id);
            if (!success) return BadRequest("Error deleting provider");

            return Ok("Provider deleted successfully");
        }

        [HttpGet("search")]
        [AllowAnonymous] // Login k baghair bhi search ho saky
        public async Task<IActionResult> Search([FromQuery] string query, [FromServices] DataAccessLayer.Interfaces.IReviewRepository reviewRepo)
        {
            if (string.IsNullOrEmpty(query))
                return BadRequest("Search query is required.");

            var providers = await _providerRepository.SearchProvidersAsync(query);

            var resultList = new List<object>();

            // Data ko clean object me convert karo (DTO projection)
            foreach (var p in providers)
            {
                // Rating fetch karna (Agar Review Repo available hai)
                var avgRating = 0.0;
                if (reviewRepo != null)
                {
                    avgRating = await reviewRepo.GetAverageRatingForProviderAsync(p.Id);
                }

                resultList.Add(new
                {
                    ProviderId = p.Id,
                    Name = $"{p.FirstName} {p.LastName}",
                    CategoryName = p.Category?.Title ?? "General",
                    City = p.City,
                    HourlyRate = p.HourlyRate,
                    Bio = p.Bio,
                    Rating = avgRating,
                });
            }

            return Ok(resultList);
        }
    }
}
