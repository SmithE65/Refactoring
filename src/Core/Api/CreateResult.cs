namespace Core.Api;

public record class CreateResult<T>(int Id, T Data);
