using ContactsAPI.Application.Features.AllPerson.Commands;
using ContactsAPI.Application.Interfaces;
using ContactsAPI.Domain.Models;
using MediatR;

namespace ContactsAPI.Application.Features.Contacts.Handlers;

public class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, CommandResult>
{
	private readonly ICrudRepository<Person> _personRepository;

	public GetPersonByIdQueryHandler(ICrudRepository<Person> personRepository)
	{
		_personRepository = personRepository;
	}

	public async Task<CommandResult> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
	{
		var person = await _personRepository.GetByIdAsync(request.Id, cancellationToken);
		if (person == null)
			return new CommandResult(CommandStatus.NotFound, message: $"Could not find person with id: {request.Id}");

		return new CommandResult(CommandStatus.Success, value: person);
	}
}
