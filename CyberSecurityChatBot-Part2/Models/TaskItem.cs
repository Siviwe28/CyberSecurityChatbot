using System.ComponentModel.DataAnnotations;

namespace CyberSecurityChatbotWPF.Models
{
    public class TaskItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string Reminder { get; set; }

        public bool IsComplete { get; set; }

        [MaxLength(50)]
        public string CreatedAt { get; set; }
    }
}