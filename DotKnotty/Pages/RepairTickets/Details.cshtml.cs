using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DotKnotty.Data;
using DotKnotty.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;

namespace DotKnotty.Pages.RepairTickets
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DetailsModel> _logger;

        public DetailsModel(ApplicationDbContext context, ILogger<DetailsModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public DotKnotty.Models.RepairTicket RepairTicket { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Direct Object Reference Vulnerability: No check for user ownership
            var repairTicket = await _context.RepairTickets
                .Include(r => r.ShipConfiguration)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (repairTicket == null)
            {
                return NotFound();
            }

            RepairTicket = repairTicket;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("OnPostAsync started");

            if (!TryValidateModel(RepairTicket, nameof(RepairTicket)))
            {
                _logger.LogWarning("ModelState is invalid");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        _logger.LogWarning($"Model error: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            _logger.LogInformation($"Updating repair ticket: {RepairTicket.Id}");
            _logger.LogInformation($"New description: {RepairTicket.Description}");

            // Fetch the existing ticket
            var existingTicket = await _context.RepairTickets.FindAsync(RepairTicket.Id);
            if (existingTicket == null)
            {
                return NotFound();
            }

            // Store original values of fields we want to protect from unintended updates
            var originalShipConfigurationId = existingTicket.ShipConfigurationId;
            var originalUserId = existingTicket.UserId;
            var originalCreatedAt = existingTicket.CreatedAt;
            var originalComponent = existingTicket.Component;
            var originalPrice = existingTicket.Price;

            // Explicitly update the description
            existingTicket.Description = RepairTicket.Description;

            // Mass Assignment Vulnerability: 
            // Update all properties from the bound model, potentially allowing unintended updates
            _context.Entry(existingTicket).CurrentValues.SetValues(RepairTicket);

            // Protect certain fields from unintended updates
            existingTicket.ShipConfigurationId = originalShipConfigurationId;
            existingTicket.UserId = originalUserId;
            existingTicket.CreatedAt = originalCreatedAt;
            existingTicket.Component = originalComponent;

            // Only update the price if it was changed in the bound model
            if (RepairTicket.Price == 0 || RepairTicket.Price == originalPrice)
            {
                existingTicket.Price = originalPrice;
            }

            try
            {
                var result = await _context.SaveChangesAsync();
                _logger.LogInformation($"SaveChangesAsync result: {result}");

                if (result > 0)
                {
                    _logger.LogInformation($"Repair ticket updated successfully. ID: {RepairTicket.Id}");
                    return RedirectToPage("./Details", new { id = RepairTicket.Id });
                }
                else
                {
                    _logger.LogWarning("SaveChangesAsync returned 0. No changes were saved to the database.");
                    ModelState.AddModelError(string.Empty, "Failed to update the repair ticket. Please try again.");
                    return Page();
                }
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "An error occurred while updating the repair ticket");
                ModelState.AddModelError(string.Empty, "An error occurred while updating the repair ticket. Please try again.");
                return Page();
            }
        }

        private bool RepairTicketExists(int id)
        {
            return _context.RepairTickets.Any(e => e.Id == id);
        }
    }
}