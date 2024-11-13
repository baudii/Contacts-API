using MediatR;

namespace ContactsAPI.Application.Features;

public class InitializeDatabaseRequest : IRequest
{
	public string? ConnectionString { get; }

	public InitializeDatabaseRequest(string? connectionString)
	{
		ConnectionString = connectionString;
	}
}
