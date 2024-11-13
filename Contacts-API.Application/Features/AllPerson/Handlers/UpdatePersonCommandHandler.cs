using ContactsAPI.Application.Features.AllPerson.Commands;
using ContactsAPI.Application.Features.Contacts.Commands;
using ContactsAPI.Application.Interfaces;
using ContactsAPI.Domain.Models;
using MediatR;

namespace ContactsAPI.Application.Features.Contacts.Handlers;
public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, CommandResult>
{
	private readonly ICrudRepository<Person> _personRepository;
	public UpdatePersonCommandHandler(ICrudRepository<Person> personRepository)
	{
		_personRepository = personRepository;
	}

	public async Task<CommandResult> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
	{
		var person = await _personRepository.GetByIdAsync(request.Id, cancellationToken);
		if (person == null)
			return new CommandResult(CommandStatus.NotFound);

		person.FullName = request.FullName;
		person.Birthdate = request.BirthDate;

		await _personRepository.UpdateAsync(person, cancellationToken);

		return new CommandResult(CommandStatus.Success);
	}
}
