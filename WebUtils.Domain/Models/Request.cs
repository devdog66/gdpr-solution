using System.ComponentModel.DataAnnotations;

namespace WebUtils.Domain.Models
{
    public class Request
    {
        [Key]
        public long Id { get; set; }

        [MaxLength(36)]
        public string? SessionId { get; set; }

        [MaxLength(1000)]
        public string? Host { get; set; }

        [Required]
        [MaxLength(4000)]
        public required string RequestPath { get; set; }

        [Required]
        [MaxLength(100)]
        public required string RequestMethod { get; set; }

        [MaxLength(4000)]
        public string? Referrer { get; set; }

        [MaxLength(4000)]
        public string? RequestCookies { get; set; }

        [MaxLength(10)]
        public string? ResponseCode { get; set; }

        [MaxLength(4000)]
        public string? ResponseCookies { get; set; }
    }
}
