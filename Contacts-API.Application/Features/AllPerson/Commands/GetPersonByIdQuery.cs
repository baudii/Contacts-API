using MediatR;

namespace ContactsAPI.Application.Features.AllPerson.Commands;

public record GetPersonByIdQuery(int Id) : IRequest<CommandResult>;
