using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DotKnotty.Data;
using DotKnotty.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DotKnotty.Pages.RepairTickets
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<RepairTicket> RepairTickets { get; set; } = new List<RepairTicket>();

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            RepairTickets = await _context.RepairTickets
                .Include(r => r.ShipConfiguration)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
    }
}