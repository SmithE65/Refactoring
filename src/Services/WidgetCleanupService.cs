using Core.Data;
using Core.Models;

namespace Services;

public class WidgetCleanupService : IWidgetCleanupService
{
    private readonly IRepository<Widget> _repo;

    public WidgetCleanupService(IRepository<Widget> repo)
    {
        _repo = repo;
    }

    public async Task RemoveDuplicatesAsync()
    {
        var step3 = await _repo.GetAllAsync();
        var groups = step3.GroupBy(x => x.Name).Where(x => x.Count() > 1);
        foreach (var grouping in groups)
        {
            var toRemove = grouping.OrderBy(x => x.Properties.Count).Skip(1);
            await _repo.DeleteManyAsync(toRemove.Select(x => x.Id));
        }
    }

    public async Task RemoveEmptyPropsAsync()
    {
        var step2 = await _repo.GetManyAsync(x => x.Properties.Any(p => string.IsNullOrWhiteSpace(p.Data)));
        foreach (var widget in step2)
        {
            widget.Properties = widget.Properties.Where(p => !string.IsNullOrWhiteSpace(p.Data)).ToList();
        }
        await _repo.SaveChangesAsync();
    }

    public async Task RemoveNoPropWidgetsAsync()
    {
        var step1 = await _repo.GetManyAsync(x => x.Properties.Count == 0);
        await _repo.DeleteManyAsync(step1.Select(x => x.Id));
    }
}
