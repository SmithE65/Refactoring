using Api.Controllers;
using Api.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Api.Tests;

public class WidgetControllerTests
{
    private readonly WidgetController _sut;

    public WidgetControllerTests()
    {
        _sut = new WidgetController(new Data.WidgetContext());
    }

    [Fact]
    public void Create_NewRecord()
    {

    }

    [Fact]
    public void Delete_RemovesRecord()
    {

    }

    [Fact]
    public void Get_ReturnsExisting()
    {

    }

    [Fact]
    public void Get_ReturnsAll()
    {
        // Act
        var result = _sut.Get().Result;

        // Assert
        Assert.NotNull(result);
        var ok = Assert.IsType<OkObjectResult>(result);
        var data = Assert.IsType<WidgetDto[]>(ok.Value);
        Assert.NotEmpty(data);
    }

    [Fact]
    public void Update_DbObjectMatchesDto()
    {

    }

    [Fact]
    public void CleanupWidgets_RemovesDuplicates()
    {

    }
}
