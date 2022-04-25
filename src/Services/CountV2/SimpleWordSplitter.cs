using System.Collections;

namespace Services.CountV2;

public class SimpleWordSplitter : IWordCollection
{
    public IEnumerable<char> _source;

    public SimpleWordSplitter(IEnumerable<char> source)
    {
        _source = source;
    }

    public IEnumerator<string> GetEnumerator()
    {
        string current = string.Empty;
        foreach (var character in _source)
        {
            if (char.IsWhiteSpace(character) && !string.IsNullOrWhiteSpace(current))
            {
                yield return current.Trim();
                current = string.Empty;
            }
            current += character;
        }

        if (!string.IsNullOrWhiteSpace(current))
        {
            yield return current.Trim();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
