namespace VehiclesApi.Exceptions;

public class ObjectNotFoundException : Exception
{
	public ObjectNotFoundException() : base("The requested object was not found.")
	{
	}

	public ObjectNotFoundException(string message) : base(message)
	{
	}

	public ObjectNotFoundException(string message, Exception innerException) : base(message, innerException)
	{
	}
}