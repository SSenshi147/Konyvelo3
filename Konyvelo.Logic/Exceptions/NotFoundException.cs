namespace Konyvelo.Logic.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException()
    {
    }

    public NotFoundException(int id) : base($"not found entity with id: {id}")
    {
    }
    
    public NotFoundException(int id, string entityType) : base($"not found {entityType} with id: {id}")
    {
    }
}