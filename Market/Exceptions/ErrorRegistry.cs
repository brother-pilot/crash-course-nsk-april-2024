namespace Market.Exceptions;

public static class ErrorRegistry
{
    public static DomainException InternalServerError() =>
        new(500, "Internal server error");

    public static DomainException UserNotAuthenticated() =>
        new(401, $"User not authenticated.");

    public static DomainException NotFound(string entity, object id) =>
        new(404, $"Entity '{entity}' with id '{id}' not found");
}