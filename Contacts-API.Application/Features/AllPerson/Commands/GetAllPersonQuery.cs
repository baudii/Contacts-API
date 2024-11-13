using MediatR;

namespace ContactsAPI.Application.Features.AllPerson.Commands;

public record GetAllPersonQuery : IRequest<CommandResult>;
