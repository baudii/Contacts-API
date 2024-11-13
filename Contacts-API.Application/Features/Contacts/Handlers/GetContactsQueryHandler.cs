using ContactsAPI.Application.Features.Contacts.Commands;
using ContactsAPI.Application.Interfaces;
using ContactsAPI.Domain.Models;
using MediatR;

namespace ContactsAPI.Application.Features.Contacts.Handlers;

public class GetContactsQueryHandler : IRequestHandler<GetContactsQuery, CommandResult>
{
	private readonly ICrudRepository<Contact> _contactRepository;

	public GetContactsQueryHandler(ICrudRepository<Contact> contactRepository)
	{
		_contactRepository = contactRepository;
	}

	public async Task<CommandResult> Handle(GetContactsQuery request, CancellationToken cancellationToken)
	{
		var contacts = await _contactRepository.GetAllAsync(cancellationToken);
		return new CommandResult(CommandStatus.Success, value: contacts);
	}
}
