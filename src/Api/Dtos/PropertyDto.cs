using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public class PropertyDto
{
    [Required]
    public string Name { get; set; } = null!;
    public string? Data { get; set; }
}
