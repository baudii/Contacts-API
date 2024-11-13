using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ContactsAPI.Application.Features.Contacts.Commands;

public record GetContactByIdQuery(
	[Range(1, int.MaxValue, ErrorMessage = "Id must be positive integer")]
	int Id
) : IRequest<CommandResult>;
