using Services.CountV1;
using System.IO;
using System.Text;

namespace Services.Tests;

public class WordCountingServiceTests
{
    [Theory]
    [InlineData("one two three", 3)]
    [InlineData("one", 1)]
    public void CountsSimpleStrings(string input, int count)
    {
        // Arrange
        var sut = new WordCountingService();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));

        // Act
        var result = sut.Count(stream);

        // Assert
        Assert.Equal(count, result.Count);
    }

    [Theory]
    [InlineData("<div>This is text.</div>", 3)]
    [InlineData("<div> <h1>Test </h1>\n\t</div>", 1)]
    public void CountsHtmlDisplayText(string input, int count)
    {
        // Arrange
        var sut = new WordCountingService();
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));

        // Act
        var result = sut.Count(stream);

        // Assert
        Assert.Equal(count, result.Count);
    }
}
