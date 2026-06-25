using System.ComponentModel.DataAnnotations;

namespace CyberSecurityChatbotWPF.Models
{
    public class LogEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(50)]
        public string CreatedAt { get; set; }
    }
}