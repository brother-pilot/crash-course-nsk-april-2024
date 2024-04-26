namespace Market.DAL;
//все свойства будут init. record более сокращенная форма для иницилизации
public record DbResult(DbResultStatus Status)
{
    internal static DbResult Ok() => new(DbResultStatus.Ok);
    internal static DbResult NotFound() => new(DbResultStatus.NotFound);
    internal static DbResult UnknownError() => new(DbResultStatus.UnknownError);
}

public record DbResult<T>(T Result, DbResultStatus Status)
{
    internal static DbResult<T> Ok(T Value) => new(Value, DbResultStatus.Ok);
    internal static DbResult<T> NotFound() => new(default!, DbResultStatus.NotFound);
    internal static DbResult<T> UnknownError() => new(default!, DbResultStatus.UnknownError);
}