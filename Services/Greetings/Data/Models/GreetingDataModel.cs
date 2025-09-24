using System.ComponentModel.DataAnnotations;

namespace Onion.Assignment.Services.Greetings.Data.Models;

public class GreetingDataModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Message { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}