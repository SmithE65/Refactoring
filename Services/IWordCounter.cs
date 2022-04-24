namespace Services;

public interface IWordCounter<T>
{
    Dictionary<string, int> Count(T input);
}
