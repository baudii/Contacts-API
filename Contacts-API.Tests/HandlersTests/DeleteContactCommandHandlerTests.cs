using ContactsAPI.Application.Features;
using ContactsAPI.Application.Features.Contacts.Commands;
using ContactsAPI.Application.Features.Contacts.Handlers;
using ContactsAPI.Domain.Models;
using ContactsAPI.Infrastructure.Repositories;
using FluentAssertions;

namespace ContactsAPI.Tests.HandlersTests;

public class DeleteContactCommandHandlerTests
{

	[Fact]
	public async Task Handle_ShouldReturnCommandResultNotFound_WhenContactDoesNotExist()
	{
		// Arrange
		var context = await TestUtils.GetTemporaryDbContextAsync();
		var contactRepository = new ContactRepository(context);
		var handler = new DeleteContactByIdCommandHandler(contactRepository);

		int id = 9999;
		var command = new DeleteContactByIdCommand(id);

		// Act
		var commandResult = await handler.Handle(command, CancellationToken.None);

		// Assert
		commandResult.Status.Should().Be(CommandStatus.NotFound);

		await context.Database.EnsureDeletedAsync();
		context.Dispose();
	}


	[Fact]
	public async Task Handle_ShouldReturnSuccess_WhenContactDeletedSuccessfully()
	{
		// Arrange
		var context = await TestUtils.GetTemporaryDbContextAsync();
		var contactRepository = new ContactRepository(context);
		var handler = new DeleteContactByIdCommandHandler(contactRepository);

		int id = 1;
		var command = new DeleteContactByIdCommand(id);

		// Act
		var commandResult = await handler.Handle(command, CancellationToken.None);

		// Assert
		commandResult.Status.Should().Be(CommandStatus.Success);
		commandResult.Value.Should().BeOfType(typeof(Contact));

		await context.Database.EnsureDeletedAsync();
		context.Dispose();
	}
}
