namespace Konyvelo.Logic.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(int id, string entityType) : base($"not found {entityType} with id: {id}")
    {
    }
}