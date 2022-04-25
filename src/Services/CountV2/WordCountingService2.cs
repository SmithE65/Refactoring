namespace Services.CountV2;

public class WordCountingService2 : IWordCounter<IWordCollection>
{
    public Dictionary<string, int> Count(IWordCollection words)
    {
        var wordCounts = new Dictionary<string, int>();
        
        foreach (var word in words)
        {
            if (!wordCounts.TryAdd(word,1))
            {
                ++wordCounts[word];
            }
        }

        return wordCounts;
    }
}
