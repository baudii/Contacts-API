using ContactsAPI.Application.Features.Contacts.Commands;
using ContactsAPI.Application.Interfaces;
using ContactsAPI.Domain.Models;
using MediatR;

namespace ContactsAPI.Application.Features.Contacts.Handlers;

public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, CommandResult>
{
	private readonly ICrudRepository<Contact> _contactRepository;

	public GetContactByIdQueryHandler(ICrudRepository<Contact> contactRepository)
	{
		_contactRepository = contactRepository;
	}

	public async Task<CommandResult> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
	{
		var contact = await _contactRepository.GetByIdAsync(request.Id, cancellationToken);
		if (contact == null)
			return new CommandResult(CommandStatus.NotFound, message: $"Could not find contact with id: {request.Id}");

		return new CommandResult(CommandStatus.Success, value: contact);
	}
}
