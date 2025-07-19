using System;
using System.ComponentModel.DataAnnotations;

namespace WebUtils.Domain.Models
{
    public class Consent
    {
        [Key]
        public long Id { get; set; }

        [MaxLength(36)]
        public string? Uid { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        [MaxLength(5000)]
        public string? CookiePreferences { get; set; }
    }
}
