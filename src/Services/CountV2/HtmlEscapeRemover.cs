using System.Collections;

namespace Services.CountV2;

public class HtmlEscapeRemover : IEnumerable<char>
{
    private readonly IEnumerable<char> _source;

    public HtmlEscapeRemover(IEnumerable<char> source) => _source = source;

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
        if (enumerator.Current != '&')
        {
            return true;
        }

        while (enumerator.MoveNext() && enumerator.Current != ';')
            ;

        return enumerator.MoveNext();
    }
}

