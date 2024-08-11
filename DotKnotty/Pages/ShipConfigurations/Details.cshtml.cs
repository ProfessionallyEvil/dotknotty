using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DotKnotty.Services;
using System;
using System.Text;
using Microsoft.Extensions.Logging;

namespace DotKnotty.Pages.ShipConfigurations
{
    public class DetailsModel : PageModel
    {
        private readonly ShipConfigurationService _shipConfigService;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(ShipConfigurationService shipConfigService, ILogger<DetailsModel> logger)
        {
            _shipConfigService = shipConfigService;
            _logger = logger;
        }

        public DotKnotty.Models.ShipConfiguration? ShipConfig { get; set; }

        public IActionResult OnGet(string config)
        {
            if (string.IsNullOrEmpty(config))
            {
                _logger.LogWarning("Attempt to access Details page with empty configuration");
                return NotFound("No configuration provided");
            }

            try
            {
                ShipConfig = _shipConfigService.LoadConfiguration(config);
                Console.WriteLine($"Deserialized Ship Name: {ShipConfig.ShipName}");

                if (ShipConfig == null)
                {
                    _logger.LogWarning("LoadConfiguration returned null for config: {Config}", config);
                    return NotFound("Configuration not found");
                }

                return Page();
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Invalid Base64 string: {Config}", config);
                return BadRequest("Invalid Base64 string in configuration data");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing configuration: {Config}", config);
                return BadRequest($"Error processing configuration data: {ex.Message}");
            }
        }
    }
}