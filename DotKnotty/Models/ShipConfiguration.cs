using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Runtime.Serialization; // Add this for StreamingContext

namespace DotKnotty.Models
{
    [Serializable]
    public class ShipConfiguration
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [StringLength(100, MinimumLength = 1)]
        public string ShipName { get; set; } = string.Empty;

        [Range(0, 10)]
        public int WeaponsLevel { get; set; }

        [Range(0, 10)]
        public int ShieldLevel { get; set; }

        [NonSerialized]
        private string? _serializedConfig;

        [NotMapped]
        public string? SerializedConfig
        {
            get => _serializedConfig;
            set => _serializedConfig = value;
        }
    }
}