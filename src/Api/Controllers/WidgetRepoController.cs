using Api.Dtos;
using AutoMapper;
using Core.Data;
using Core.Models;
using Services;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WidgetRepoController : ControllerBase
{
    private readonly IRepository<Widget> _repo;
    private readonly IMapper _mapper;

    public WidgetRepoController(IRepository<Widget> repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> Create(WidgetDto dto)
    {
        var widget = _mapper.Map<WidgetDto, Widget>(dto);
        await _repo.CreateAsync(widget);
        var result = _mapper.Map<WidgetDto>(widget);
        return Created($"{Url.ActionLink()}/{widget.Id}", result);
    }

    [HttpDelete("id")]
    public async Task<IActionResult> Delete(int id)
    {
        var widget = await _repo.GetOneAsync(id);

        if (widget is null)
        {
            return NotFound();
        }

        await _repo.DeleteAsync(widget);
        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var widget = await _repo.GetOneAsync(id);

        if (widget is null)
        {
            return NotFound();
        }

        var dtos = _mapper.Map<Widget, WidgetDto>(widget);
        return Ok(dtos);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var widgets = await _repo.GetAllAsync();
        var dtos = _mapper.Map<Widget[], WidgetDto[]>(widgets);
        return Ok(dtos);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, WidgetDto dto)
    {
        var widget = await _repo.GetOneAsync(id);

        if (widget is null)
        {
            return NotFound();
        }

        widget.Name = dto.Name;
        foreach (var p in dto.Properties)
        {
            var update = widget.Properties.FirstOrDefault(x => x.Name == p.Name);

            if (update is null)
            {
                widget.Properties.Add(_mapper.Map<Property>(p));
            }
            else
            {
                update.Data = p.Data;
            }
        }

        widget.Properties = widget.Properties.Where(x => dto.Properties.Any(p => p.Name == x.Name)).ToList();
        await _repo.UpdateAsync(widget);

        return Ok();
    }

    [HttpPost("cleanup")]
    public async Task<IActionResult> CleanupWidgets([FromServices] IWidgetCleanupService service)
    {
        await service.RemoveNoPropWidgetsAsync();
        await service.RemoveEmptyPropsAsync();
        await service.RemoveDuplicatesAsync();

        return Ok();
    }
}