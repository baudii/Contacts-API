using MediatR;
using System.ComponentModel.DataAnnotations;

namespace ContactsAPI.Application.Features.Contacts.Commands;

public record CreateContactCommand(
	[Required(ErrorMessage = "Telephone number is required")]
	[MaxLength(15, ErrorMessage = "Telephone number can't be longer than 15 characters")]
	string TelephoneNumber,
	[EmailAddress(ErrorMessage = "Invalid Email!")]
	string Email,
	string LinkedIn,
	[Range(1, int.MaxValue, ErrorMessage = "PersonId must be positive integer")]
	int PersonId
) : IRequest<CommandResult>;
