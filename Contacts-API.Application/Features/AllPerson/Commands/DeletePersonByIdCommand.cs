using MediatR;

namespace ContactsAPI.Application.Features.AllPerson.Commands;

public record DeletePersonByIdCommand(int Id) : IRequest<CommandResult>;
