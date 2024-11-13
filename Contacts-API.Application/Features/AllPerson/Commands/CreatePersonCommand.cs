using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ContactsAPI.Application.Features.AllPerson.Commands;

public record CreatePersonCommand(
	[Required(ErrorMessage = "Name is required")]
	string FullName,
	[DataType(DataType.Date, ErrorMessage = "Invalid Date!")]
	DateOnly BirthDate
) : IRequest<CommandResult>;
