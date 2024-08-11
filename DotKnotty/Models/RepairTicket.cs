using System.ComponentModel.DataAnnotations;

namespace DotKnotty.Models
{
    public class RepairTicket
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int ShipConfigurationId { get; set; }
        
        public ShipConfiguration? ShipConfiguration { get; set; }
    }
}