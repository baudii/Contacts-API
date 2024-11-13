using ContactsAPI.Application.Features.AllPerson.Commands;
using ContactsAPI.Application.Interfaces;
using ContactsAPI.Domain.Models;
using MediatR;

namespace ContactsAPI.Application.Features.Contacts.Handlers;
public class DeletePersonByIdCommandHandler : IRequestHandler<DeletePersonByIdCommand, CommandResult>
{
	private readonly ICrudRepository<Person> _personRepository;
	public DeletePersonByIdCommandHandler(ICrudRepository<Person> personRepository)
	{
		_personRepository = personRepository;
	}

	public async Task<CommandResult> Handle(DeletePersonByIdCommand request, CancellationToken cancellationToken)
	{
		var person = await _personRepository.DeleteAsync(request.Id, cancellationToken);
		if (person == null)
			return new CommandResult(CommandStatus.NotFound);

		return new CommandResult(CommandStatus.Success, value: person);
	}
}