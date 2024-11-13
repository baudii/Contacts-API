using ContactsAPI.Application.Features.Contacts.Commands;
using ContactsAPI.Application.Interfaces;
using ContactsAPI.Domain.Models;
using MediatR;

namespace ContactsAPI.Application.Features.Contacts.Handlers;
public class DeleteContactByIdCommandHandler : IRequestHandler<DeleteContactByIdCommand, CommandResult>
{
	private readonly ICrudRepository<Contact> _contactRepository;
	public DeleteContactByIdCommandHandler(ICrudRepository<Contact> contactRepository)
	{
		_contactRepository = contactRepository;
	}

	public async Task<CommandResult> Handle(DeleteContactByIdCommand request, CancellationToken cancellationToken)
	{
		var contact = await _contactRepository.DeleteAsync(request.Id, cancellationToken);
		if (contact == null)
			return new CommandResult(CommandStatus.NotFound);

		return new CommandResult(CommandStatus.Success, value: contact);
	}
}