using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DotKnotty.Data;
using DotKnotty.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace DotKnotty.Pages.RepairTickets
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewData["ShipConfigurations"] = new SelectList(_context.ShipConfigurations.Where(s => s.UserId == userId), "Id", "ShipName");
            return Page();
        }

        [BindProperty]
        public DotKnotty.Models.RepairTicket RepairTicket { get; set; } = new DotKnotty.Models.RepairTicket();

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                _logger.LogInformation("OnPostAsync started");

                var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    _logger.LogError("User ID claim is null or empty");
                    ModelState.AddModelError(string.Empty, "Unable to identify the current user. Please try logging out and back in.");
                    return Page();
                }

                RepairTicket.UserId = userIdClaim;

                // Remove UserId from ModelState as we've set it manually
                ModelState.Remove("RepairTicket.UserId");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("ModelState is invalid");
                    foreach (var modelState in ModelState.Values)
                    {
                        foreach (var error in modelState.Errors)
                        {
                            _logger.LogWarning($"Model error: {error.ErrorMessage}");
                        }
                    }
                    ViewData["ShipConfigurations"] = new SelectList(_context.ShipConfigurations.Where(s => s.UserId == userIdClaim), "Id", "ShipName");
                    return Page();
                }

                // Set the price based on the selected component
                RepairTicket.Price = RepairTicket.Component switch
                {
                    "Weapons" => 1200m,
                    "Shields" => 800m,
                    "Engines" => 1600m,
                    "Hull" => 500m,
                    _ => 0m
                };

                _logger.LogInformation($"Creating repair ticket for user: {RepairTicket.UserId}");
                _logger.LogInformation($"Repair ticket details: ShipConfigurationId: {RepairTicket.ShipConfigurationId}, Component: {RepairTicket.Component}, Price: {RepairTicket.Price}, Description: {RepairTicket.Description}");

                _context.RepairTickets.Add(RepairTicket);
                var result = await _context.SaveChangesAsync();

                _logger.LogInformation($"SaveChangesAsync result: {result}");

                if (result > 0)
                {
                    _logger.LogInformation($"Repair ticket created successfully. ID: {RepairTicket.Id}");
                    return RedirectToPage("./Index");
                }
                else
                {
                    _logger.LogWarning("SaveChangesAsync returned 0. No changes were saved to the database.");
                    ModelState.AddModelError(string.Empty, "Failed to create the repair ticket. Please try again.");
                    ViewData["ShipConfigurations"] = new SelectList(_context.ShipConfigurations.Where(s => s.UserId == userIdClaim), "Id", "ShipName");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the repair ticket");
                ModelState.AddModelError(string.Empty, "An error occurred while creating the repair ticket. Please try again.");
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                ViewData["ShipConfigurations"] = new SelectList(_context.ShipConfigurations.Where(s => s.UserId == userId), "Id", "ShipName");
                return Page();
            }
        }
    }
}