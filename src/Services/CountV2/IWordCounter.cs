namespace Services.CountV2;

public interface IWordCounter<T>
{
    Dictionary<string, int> Count(T input);
}
