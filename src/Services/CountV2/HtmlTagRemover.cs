using System.Collections;

namespace Services.CountV2;

public class HtmlTagRemover : IEnumerable<char>
{
    private readonly IEnumerable<char> _source;
    private static readonly string[] _skipTags = { "script", "style" };

    public HtmlTagRemover(IEnumerable<char> source) => _source = source;

    public IEnumerator<char> GetEnumerator()
    {
        var enumerator = _source.GetEnumerator();
        while (enumerator.MoveNext())
        {
            if (ReadPastTag(enumerator))
            {
                yield return enumerator.Current;
            }

        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private static bool ReadPastTag(IEnumerator<char> enumerator)
    {
        if (enumerator.Current != '<')
        {
            return true;
        }

        var tag = ReadTag(enumerator).ToLower();

        if (_skipTags.Contains(tag))
        {
            ReadThroughClose(enumerator, tag);
        }

        return enumerator.MoveNext();
    }

    private static void ReadThroughClose(IEnumerator<char> enumerator, string tag)
    {
        var close = $"</{tag}>";
        var idx = 0;
        while (enumerator.MoveNext())
        {
            if (enumerator.Current == close[idx])
            {
                idx++;
                continue;
            }
            else if (idx == close.Length)
            {
                break;
            }

            idx = 0;
        }
    }

    private static string ReadTag(IEnumerator<char> enumerator)
    {
        string tag = string.Empty;
        var reading = true;
        while (enumerator.MoveNext() && enumerator.Current != '>')
        {
            if (!reading)
                continue;

            if (char.IsWhiteSpace(enumerator.Current))
                reading = false;
            else
                tag += enumerator.Current;
        }

        return tag;
    }
}
