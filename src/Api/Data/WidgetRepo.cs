using Core.Data;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.Data;

internal sealed class WidgetRepo : IRepository<Widget>
{
    private readonly WidgetContext _context;

    private IQueryable<Widget> Widgets => _context.Widgets.Include(x => x.Properties);

    public WidgetRepo(WidgetContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Widget data)
    {
        await _context.AddAsync(data);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Widget widget)
    {
        _context.Remove(widget);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteManyAsync(IEnumerable<Widget> widgets)
    {
        _context.Widgets.RemoveRange(widgets);
        await _context.SaveChangesAsync();
    }

    public async Task ExecuteCommandAsync<Tc>(Tc command) where Tc : IDataCommand<Widget>
        => await command.ExecuteAsync();

    public async Task<Tr> ExecuteQueryAsync<Tq, Tr>(Tq query) where Tq : IDataQuery<Widget, Tr>
        => await query.ExecuteAsync();

    public async Task<Widget[]> GetAllAsync()
        => await Widgets.ToArrayAsync();

    public async Task<Widget[]> GetManyAsync(Expression<Func<Widget, bool>> query)
        => await Widgets.Where(query).ToArrayAsync();

    public async Task<Widget?> GetOneAsync(int id)
        => await Widgets.FirstOrDefaultAsync(x => x.Id == id);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public async Task UpdateAsync(Widget data)
    {
        _context.Update(data);
        await _context.SaveChangesAsync();
    }
}
