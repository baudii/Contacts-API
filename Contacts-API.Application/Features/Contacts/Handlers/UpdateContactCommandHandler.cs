using ContactsAPI.Application.Features.Contacts.Commands;
using ContactsAPI.Application.Interfaces;
using ContactsAPI.Domain.Models;
using MediatR;

namespace ContactsAPI.Application.Features.Contacts.Handlers;
public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, CommandResult>
{
	private readonly ICrudRepository<Contact> _contactRepository;
	private readonly ICrudRepository<Person> _personRepository;
	public UpdateContactCommandHandler(ICrudRepository<Contact> contactRepository, ICrudRepository<Person> personRepository)
	{
		_contactRepository = contactRepository;
		_personRepository = personRepository;
	}

	public async Task<CommandResult> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
	{
		var contact = await _contactRepository.GetByIdAsync(request.Id, cancellationToken);
		if (contact == null)
			return new CommandResult(CommandStatus.NotFound);

		if (contact.PersonId != request.PersonId)
		{
			var newPerson = await _personRepository.GetByIdAsync(request.PersonId, cancellationToken);
			if (newPerson == null)
				return new CommandResult(CommandStatus.BadRequest);
		}

		contact.TelephoneNumber = request.TelephoneNumber;
		contact.Email = request.Email;
		contact.LinkedIn = request.LinkedIn;
		contact.PersonId = request.PersonId;

		await _contactRepository.UpdateAsync(contact, cancellationToken);

		return new CommandResult(CommandStatus.Success);
	}
}
