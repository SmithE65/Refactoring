using Core.Data;

namespace Core.Models;

public class Property : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public string? Data { get; set; } = string.Empty;
    public Widget Widget { get; set; } = null!;
}
