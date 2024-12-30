using ContactsAPI.Application.Features;
using ContactsAPI.Application.Features.Contacts.Commands;
using ContactsAPI.Application.Features.Contacts.Handlers;
using ContactsAPI.Domain.Models;
using ContactsAPI.Infrastructure.Repositories;
using FluentAssertions;

namespace ContactsAPI.Tests.HandlersTests;

public class CreateContactCommandHandlerTests
{
	[Fact]
	public async Task Handle_ShouldReturnBadRequest_WhenPersonNotFound()
	{
		// Arrange
		var command = new CreateContactCommand("Test Title", "email@s.ro", "Test Genre", 9999);
		var context = await TestUtils.GetTemporaryDbContextAsync();
		var personRepository = new PersonRepository(context);
		var contacRepository = new ContactRepository(context);
		var handler = new CreateContactCommandHandler(contacRepository, personRepository);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Status.Should().Be(CommandStatus.BadRequest);
		result.Message.Should().BeOfType(typeof(string));

		await context.Database.EnsureDeletedAsync();
		context.Dispose();
	}

	[Fact]
	public async Task Handle_ShouldReturnBadRequest_WhenContactAlreadyExists()
	{
		// Arrange
		var contact = TestUtils.GetStandardContact();
		contact.Id = 1;
		var command = new CreateContactCommand(contact.TelephoneNumber, contact.Email, contact.LinkedIn, contact.PersonId);
		var context = await TestUtils.GetTemporaryDbContextAsync();
		var personRepository = new PersonRepository(context);
		var contacRepository = new ContactRepository(context);
		var handler = new CreateContactCommandHandler(contacRepository, personRepository);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Status.Should().Be(CommandStatus.BadRequest);
		result.Message.Should().BeOfType(typeof(string));

		await context.Database.EnsureDeletedAsync();
		context.Dispose();
	}


	[Fact]
	public async Task Handle_ShouldReturnSuccess_WhenContactCreatedSuccessfully()
	{
		// Arrange
		Contact contact = new()
		{
			TelephoneNumber = "Unique Titile",
			Email = "someemail@ams.ss",
			LinkedIn = "Unique Genre",
			PersonId = 1
		};
		var command = new CreateContactCommand(contact.TelephoneNumber, contact.Email, contact.LinkedIn, contact.PersonId);
		var context = await TestUtils.GetTemporaryDbContextAsync();
		var personRepository = new PersonRepository(context);
		var contactRepository = new ContactRepository(context);
		var handler = new CreateContactCommandHandler(contactRepository, personRepository);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Status.Should().Be(CommandStatus.Success);
		result.Value.Should().BeOfType(typeof(Contact));
		result.Value.Should().BeEquivalentTo(contact, options => options.Excluding(b => b.Id).Excluding(b => b.Person));

		await context.Database.EnsureDeletedAsync();
		context.Dispose();
	}
}
