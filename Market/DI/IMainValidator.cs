namespace Market.DI;

public interface IMainValidator
{
    Task Validate<T>(T value);
}