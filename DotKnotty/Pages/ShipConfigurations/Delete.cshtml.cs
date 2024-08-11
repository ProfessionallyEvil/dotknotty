using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DotKnotty.Data;
using DotKnotty.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DotKnotty.Pages.ShipConfigurations
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DotKnotty.Models.ShipConfiguration ShipConfiguration { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipConfiguration = await _context.ShipConfigurations.FirstOrDefaultAsync(m => m.Id == id);

            if (shipConfiguration == null)
            {
                return NotFound();
            }
            else
            {
                ShipConfiguration = shipConfiguration;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipConfiguration = await _context.ShipConfigurations.FindAsync(id);

            if (shipConfiguration != null)
            {
                ShipConfiguration = shipConfiguration;
                _context.ShipConfigurations.Remove(ShipConfiguration);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}