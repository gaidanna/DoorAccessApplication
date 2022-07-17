using System.ComponentModel.DataAnnotations;

namespace DoorAccessApplication.Ids.Models
{
    public record ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; init; }
    }
}
