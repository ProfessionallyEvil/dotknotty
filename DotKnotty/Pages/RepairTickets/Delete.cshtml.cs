using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DotKnotty.Data;
using DotKnotty.Models;
using Microsoft.AspNetCore.Authorization;

namespace DotKnotty.Pages.RepairTickets
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
        public DotKnotty.Models.RepairTicket RepairTicket { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairticket = await _context.RepairTickets
                .Include(r => r.ShipConfiguration)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (repairticket == null)
            {
                return NotFound();
            }
            else
            {
                RepairTicket = repairticket;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repairticket = await _context.RepairTickets.FindAsync(id);
            if (repairticket != null)
            {
                RepairTicket = repairticket;
                _context.RepairTickets.Remove(RepairTicket);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}