using ContactsAPI.Application.Features.AllPerson.Commands;
using ContactsAPI.Application.Interfaces;
using ContactsAPI.Domain.Models;
using MediatR;

namespace ContactsAPI.Application.Features.Contacts.Handlers;

public class CreatePersonCommandHandler : IRequestHandler<CreatePersonCommand, CommandResult>
{
	private readonly ICrudRepository<Person> _personRepository;
	public CreatePersonCommandHandler(ICrudRepository<Contact> context, ICrudRepository<Person> personRepository)
	{
		_personRepository = personRepository;
	}

	public async Task<CommandResult> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
	{
		var newPerson = new Person
		{
			FullName = request.FullName,
			Birthdate = request.BirthDate
		};

		var person = await _personRepository.AddAsync(newPerson, cancellationToken);

		if (person == null)
			return new CommandResult(CommandStatus.Internal500, message: $"Something went wrong when creating person. Person was returned null.");

		return new CommandResult(CommandStatus.Success, value: person);
	}
}
