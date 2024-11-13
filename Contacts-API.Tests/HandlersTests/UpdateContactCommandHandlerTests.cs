using ContactsAPI.Application.Features;
using ContactsAPI.Application.Features.Contacts.Commands;
using ContactsAPI.Application.Features.Contacts.Handlers;
using ContactsAPI.Infrastructure.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Tests.HandlersTests;

public class UpdateContactCommandHandlerTests
{
	[Fact]
	public async Task Handle_ShouldReturnNotFound_WhenContactNotFound()
	{
		// Arrange
		var context = await TestUtils.GetTemporaryDbContextAsync();
		var contactRepository = new ContactRepository(context);
		var personRepository = new PersonRepository(context);
		var handler = new UpdateContactCommandHandler(contactRepository, personRepository);

		int id = 9999;
		var command = new UpdateContactCommand(id, "Some", "sww@s", "Linked", 1);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Status.Should().Be(CommandStatus.NotFound);

		await context.Database.EnsureDeletedAsync();
		context.Dispose();
	}

	[Fact]
	public async Task Handle_ShouldReturnBadRequest_WhenPersonNotFound()
	{
		// Arrange
		var context = await TestUtils.GetTemporaryDbContextAsync();
		var contactRepository = new ContactRepository(context);
		var personRepository = new PersonRepository(context);
		var handler = new UpdateContactCommandHandler(contactRepository, personRepository);

		var existingContact = await context.Contacts.FirstOrDefaultAsync();
		int nonExistentPersonId = 9999;
		var command = new UpdateContactCommand(existingContact!.Id, "Updated Title", "sww@s", "Updated Genre", nonExistentPersonId);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Status.Should().Be(CommandStatus.BadRequest);

		await context.Database.EnsureDeletedAsync();
		context.Dispose();
	}

	[Fact]
	public async Task Handle_ShouldReturnSuccess_WhenContactUpdatedSuccessfully()
	{
		// Arrange
		var context = await TestUtils.GetTemporaryDbContextAsync();
		var contactRepository = new ContactRepository(context);
		var personRepository = new PersonRepository(context);
		var handler = new UpdateContactCommandHandler(contactRepository, personRepository);

		var existingContacts = await context.Contacts.FirstOrDefaultAsync();
		var existingPerson = await context.AllPerson.FirstOrDefaultAsync();
		var command = new UpdateContactCommand(existingContacts!.Id, "Updated Phone", "iupd@s", "Updated linked", existingPerson!.Id);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Status.Should().Be(CommandStatus.Success);

		var updatedContact = await context.Contacts.FindAsync(existingContacts.Id);
		updatedContact.Should().NotBeNull();
		updatedContact!.TelephoneNumber.Should().Be("Updated Phone");
		updatedContact.Email.Should().Be("iupd@s");
		updatedContact.LinkedIn.Should().Be("Updated linked");

		await context.Database.EnsureDeletedAsync();
		context.Dispose();
	}
}
