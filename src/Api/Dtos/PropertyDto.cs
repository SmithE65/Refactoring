using System.ComponentModel.DataAnnotations;

namespace Api.Dtos;

public record class PropertyDto([Required] string Name, string Data);
