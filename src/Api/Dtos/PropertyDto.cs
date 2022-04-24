using System.ComponentModel.DataAnnotations;

namespace Refactoring.Dtos;

public record class PropertyDto([Required] string Name, string Data);
