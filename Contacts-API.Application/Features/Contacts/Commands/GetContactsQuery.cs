using MediatR;

namespace ContactsAPI.Application.Features.Contacts.Commands;

public record GetContactsQuery() : IRequest<CommandResult>;
