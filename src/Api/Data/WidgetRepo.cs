using Core.Data;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.Data;

public class WidgetRepo : IRepository<Widget>
{
    private readonly WidgetContext _context;

    public WidgetRepo(WidgetContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Widget data)
    {
        await _context.AddAsync(data);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        _context.Remove(new Widget() { Id = id });
        await _context.SaveChangesAsync();
    }

    public async Task DeleteManyAsync(IEnumerable<int> ids)
    {
        _context.RemoveRange(ids.Select(x => new Widget() { Id = x }));
        await _context.SaveChangesAsync();
    }

    public async Task ExecuteCommandAsync<Tc>(Tc command) where Tc : IDataCommand<Widget>
        => await command.ExecuteAsync();

    public async Task<Tr> ExecuteQueryAsync<Tq, Tr>(Tq query) where Tq : IDataQuery<Widget, Tr>
        => await query.ExecuteAsync();

    public async Task<Widget[]> GetAllAsync()
        => await _context.Widgets.ToArrayAsync();

    public async Task<Widget[]> GetManyAsync(Expression<Func<Widget, bool>> query)
        => await _context.Widgets.Where(query).ToArrayAsync();

    public async Task<Widget?> GetOneAsync(int id)
        => await _context.Widgets.FindAsync(id);

    public async Task SaveChangesAsync()
        => await _context.SaveChangesAsync();

    public async Task UpdateAsync(Widget data)
    {
        _context.Update(data);
        await _context.SaveChangesAsync();
    }
}
