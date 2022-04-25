using Services.CountV2;
using System.Linq;

namespace Services.Tests;

public class SimpleWordSplitterTests
{
    [Fact]
    public void SingleWord()
    {
        // Arrange
        var sut = new SimpleWordSplitter("one");

        // Act
        var result = sut.ToList();

        // Assert
        Assert.Single(result);
    }
}

public class HtmlTagRemoverTests
{
    [Fact]
    public void RemovesOpenAndCloseTags()
    {
        // Arrange
        var sut = new HtmlTagRemover("<div>one</div>");

        // Act
        var result = new string(sut.ToArray());

        // Assert
        Assert.Equal("one", result);
    }
}
