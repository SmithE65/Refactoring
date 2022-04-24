using Core.Data;

namespace Core.Models;

public class Widget : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public IList<Property> Properties { get; set; } = new List<Property>();
}
