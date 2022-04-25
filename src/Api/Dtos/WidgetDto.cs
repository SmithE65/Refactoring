using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public class WidgetDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    public PropertyDto[] Properties { get; set; } = Array.Empty<PropertyDto>();
}
