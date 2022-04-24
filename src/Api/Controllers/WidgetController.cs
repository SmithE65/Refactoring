using AutoMapper;
using Core.Api;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Refactoring.Data;
using Refactoring.Dtos;

namespace Refactoring.Controllers;

[ApiController]
[Route("[controller]")]
public class WidgetController : ControllerBase
{
    private readonly WidgetContext _context;
    private readonly ILogger<WidgetController> _logger;

    public WidgetController(WidgetContext context, ILogger<WidgetController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create(WidgetDto dto)
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<WidgetDto, Widget>();
            cfg.CreateMap<Widget, WidgetDto>();
            cfg.CreateMap<PropertyDto, Property>();
            cfg.CreateMap<Property, PropertyDto>();
        });
        var mapper = new Mapper(config);
        var widget = mapper.Map<WidgetDto, Widget>(dto);
        await _context.Widgets.AddAsync(widget);
        await _context.SaveChangesAsync();

        var result = mapper.Map<WidgetDto>(widget) ?? throw new Exception("Mapping error!");
        return Created(Url.ActionLink(values: new { id = widget.Id }) ?? throw new Exception("Failed to build resource URI"), new CreateResult<WidgetDto>(widget.Id, result));
    }

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(int id)
    {
        _context.Widgets.Remove(new() { Id = id });
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var widget = await _context.Widgets.Include(x => x.Properties).FirstOrDefaultAsync(x => x.Id == id);

        if (widget is null)
        {
            return NotFound();
        }

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Widget, WidgetDto>();
            cfg.CreateMap<Property, PropertyDto>();
        });

        var dtos = new Mapper(config).Map<Widget, WidgetDto>(widget);

        return Ok(dtos);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var widgets = await _context.Widgets.Include(x => x.Properties).ToArrayAsync();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Widget, WidgetDto>();
            cfg.CreateMap<Property, PropertyDto>();
        });

        var dtos = new Mapper(config).Map<Widget[], WidgetDto[]>(widgets);

        return Ok(dtos);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, WidgetDto dto)
    {
        var widget = await _context.Widgets.FindAsync(id);

        if (widget is null)
        {
            return NotFound();
        }

        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Property, PropertyDto>();
        });

        var mapper = new Mapper(config);

        // update widget props
        widget.Name = dto.Name;
        foreach (var p in dto.Properties)
        {
            var update = widget.Properties.FirstOrDefault(x => x.Name == p.Name);

            if (update is null)
            {
                widget.Properties.Add(mapper.Map<Property>(p));
            }
            else
            {
                update.Data = p.Data;
            }
        }

        widget.Properties = widget.Properties.Where(x => dto.Properties.Any(p => p.Name == x.Name)).ToList();
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("cleanup")]
    public async Task<IActionResult> CleanupWidgets()
    {
        // delete all widgets w/o properties
        var step1 = await _context.Widgets.Where(x => x.Properties.Count == 0).ToListAsync();
        _context.Widgets.RemoveRange(step1);
        await _context.SaveChangesAsync();

        // delete all properties where Data is empty
        var step2 = await _context.Widgets.Where(x => x.Properties.Any(p => string.IsNullOrWhiteSpace(p.Data))).ToListAsync();
        foreach (var widget in step2)
        {
            widget.Properties = widget.Properties.Where(p => !string.IsNullOrWhiteSpace(p.Data)).ToList();
        }
        await _context.SaveChangesAsync();

        // remove duplicate widgets by name
        var step3 = await _context.Widgets.GroupBy(x => x.Name).Where(x => x.Count() > 1).ToListAsync();
        foreach (var grouping in step3)
        {
            var toRemove = grouping.OrderBy(x => x.Properties.Count).Skip(1);
            _context.Widgets.RemoveRange(toRemove);
        }
        await _context.SaveChangesAsync();

        return Ok();
    }
}
