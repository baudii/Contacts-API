using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ContactsAPI.Application.Features.AllPerson.Commands;

public record UpdatePersonCommand(
	[Required(ErrorMessage = "Id is required")]
	[Range(1, int.MaxValue, ErrorMessage = "Id must be positive integer")]
	int Id,
	[Required(ErrorMessage = "Name is required")]
	string FullName,
	[DataType(DataType.Date, ErrorMessage = "Invalid Date!")]
	DateOnly BirthDate
) : IRequest<CommandResult>;
