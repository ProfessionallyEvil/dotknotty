using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DotKnotty.Data;
using DotKnotty.Models;
using DotKnotty.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace DotKnotty.Pages.ShipConfigurations
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly ShipConfigurationService _shipConfigService;

        public IndexModel(ApplicationDbContext context, ILogger<IndexModel> logger, ShipConfigurationService shipConfigService)
        {
            _context = context;
            _logger = logger;
            _shipConfigService = shipConfigService;
        }

        public IList<DotKnotty.Models.ShipConfiguration> ShipConfigurations { get; set; } = new List<DotKnotty.Models.ShipConfiguration>();

        [BindProperty]
        public DotKnotty.Models.ShipConfiguration NewShipConfiguration { get; set; } = new DotKnotty.Models.ShipConfiguration();

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ShipConfigurations = await _context.ShipConfigurations
                .Where(s => s.UserId == userId)
                .ToListAsync();

            foreach (var config in ShipConfigurations)
            {
                config.SerializedConfig = GenerateSerializedConfig(config);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogError("User ID is null or empty");
                ModelState.AddModelError(string.Empty, "Unable to identify the current user. Please try logging out and back in.");
                return Page();
            }

            // Set the UserId before model validation
            NewShipConfiguration.UserId = userId;

            // Manually remove UserId from ModelState to prevent validation error
            ModelState.Remove("NewShipConfiguration.UserId");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Model state is invalid");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogWarning($"Model error: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            try
            {
                _logger.LogInformation($"Attempting to save ship configuration for user: {userId}");

                _context.ShipConfigurations.Add(NewShipConfiguration);
                
                _logger.LogInformation($"Saving changes to database");
                var saveResult = await _context.SaveChangesAsync();
                _logger.LogInformation($"SaveChangesAsync result: {saveResult}");

                if (saveResult > 0)
                {
                    _logger.LogInformation("Ship configuration saved successfully");
                }
                else
                {
                    _logger.LogWarning("No changes were saved to the database");
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving the ship configuration");
                ModelState.AddModelError(string.Empty, "An error occurred while saving the ship configuration. Please try again.");
                return Page();
            }
        }

private string GenerateSerializedConfig(DotKnotty.Models.ShipConfiguration config)
        {
            return _shipConfigService.SaveConfiguration(config);
        }
    }
}