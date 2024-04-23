namespace Market.DAL;
//все свойства будут init. record более сокращенная форма для иницилизации
internal record DbResult(DbResultStatus Status);

internal record DbResult<T>(T Result, DbResultStatus Status);