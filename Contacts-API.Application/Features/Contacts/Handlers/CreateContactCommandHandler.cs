using ContactsAPI.Application.Features.Contacts.Commands;
using ContactsAPI.Application.Interfaces;
using ContactsAPI.Domain.Models;
using MediatR;

namespace ContactsAPI.Application.Features.Contacts.Handlers;

public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, CommandResult>
{
	private readonly ICrudRepository<Contact> _contactRepository;
	private readonly ICrudRepository<Person> _personRepository;
	public CreateContactCommandHandler(ICrudRepository<Contact> context, ICrudRepository<Person> personRepository)
	{
		_contactRepository = context;
		_personRepository = personRepository;
	}

	public async Task<CommandResult> Handle(CreateContactCommand request, CancellationToken cancellationToken)
	{
		var person = await _personRepository.GetByIdAsync(request.PersonId, cancellationToken);

		if (person == null)
			return new CommandResult(CommandStatus.BadRequest, message: $"Person with id={request.PersonId} doesn't exist");


		var contacts = _contactRepository.GetAllAsync(cancellationToken);

		var contactExist = contacts.Result.Any(contact =>
			contact.TelephoneNumber == request.TelephoneNumber &&
			contact.LinkedIn == request.LinkedIn &&
			contact.Email == request.Email &&
			contact.PersonId == person.Id
		);

		if (contactExist)
			return new CommandResult(CommandStatus.BadRequest, message: $"Identical contact already exists");

		var contact = new Contact
		{
			TelephoneNumber = request.TelephoneNumber,
			LinkedIn = request.LinkedIn,
			Email = request.Email,
			PersonId = person.Id
		};

		Contact addedValue = await _contactRepository.AddAsync(contact, cancellationToken);

		return new CommandResult(CommandStatus.Success, value: addedValue);
	}
}
