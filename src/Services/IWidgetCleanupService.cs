namespace Services;

public interface IWidgetCleanupService
{
    Task RemoveDuplicatesAsync();
    Task RemoveEmptyPropsAsync();
    Task RemoveNoPropWidgetsAsync();
}
