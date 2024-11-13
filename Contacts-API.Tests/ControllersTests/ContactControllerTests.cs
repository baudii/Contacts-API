using ContactsAPI.Application.Features;
using ContactsAPI.Application.Features.Contacts.Commands;
using ContactsAPI.Controllers;
using ContactsAPI.Domain.Models;
using FakeItEasy;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContactsAPI.Tests.ControllersTests;

public class ContactControllerTests
{
	private readonly ISender _sender;
	private readonly ContactsController _contactController;

	public ContactControllerTests()
	{
		// Dependancies
		_sender = A.Fake<ISender>();

		// SUT (System Under Test)
		_contactController = new ContactsController(_sender);
	}

	#region Get.Tests

	[Fact]
	public async Task GetContacts_ReturnsOk_WhenContactsExists()
	{
		// Arrange
		var fakeContacts = new List<Contact>
		{
			new Contact { Id = 1, TelephoneNumber = "12345" },
			new Contact { Id = 2, TelephoneNumber = "123456" }
		};
		var ContactsQuery = new GetContactsQuery();
		var queryResult = new CommandResult(CommandStatus.Success, fakeContacts);
		A.CallTo(() => _sender.Send(A<GetContactsQuery>.Ignored, A<CancellationToken>.Ignored))!
			.Returns(queryResult);

		// Act
		var result = await _contactController.GetContacts();

		// Assert
		var contacts = result.Result.Should().BeOfType<OkObjectResult>().Subject;
		contacts!.Value.Should().BeEquivalentTo(fakeContacts);
	}

	[Theory]
	[InlineData(CommandStatus.NotFound)]
	[InlineData(CommandStatus.BadRequest)]
	[InlineData((CommandStatus)999)]
	public async Task GetContacts_ReturnsBadActionResult_WhenStatusNotSuccess(CommandStatus status)
	{
		// Arrange
		var commandResult = new CommandResult(status, null, null);
		var expected = commandResult.ConvertToActionResult();
		A.CallTo(() => _sender.Send(A<GetContactsQuery>.Ignored, A<CancellationToken>.Ignored))
			.Returns(commandResult);

		// Act
		var result = await _contactController.GetContacts();

		// Assert
		result.Result.Should().BeOfType(expected.GetType());
	}

	[Fact]
	public async Task GetContacts_ReturnsOkWithContact_WhenContactExists()
	{
		// Arrange
		var contactId = 1;
		var fakeContact = new Contact
		{
			Id = contactId,
			TelephoneNumber = "Test"
		};
		var ContactQueryById = new GetContactByIdQuery(contactId);
		var queryResult = new CommandResult(CommandStatus.Success, value: fakeContact);

		A.CallTo(() => _sender.Send(A<GetContactByIdQuery>.That.Matches(query => query.Id == contactId), A<CancellationToken>.Ignored))
			.Returns(queryResult);

		// Act
		var result = await _contactController.GetContact(contactId);

		// Assert
		var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
		okResult!.Value.Should().BeEquivalentTo(fakeContact);
	}

	[Theory]
	[InlineData(CommandStatus.NotFound)]
	[InlineData(CommandStatus.NotFound, "Some Msg")]
	[InlineData(CommandStatus.BadRequest)]
	[InlineData(CommandStatus.BadRequest, "Some Msg")]
	[InlineData((CommandStatus)999)]
	public async Task GetContact_ReturnsBadActionResult_WhenStatusNotSuccess(CommandStatus status, string? message = null)
	{
		// Arrange
		var contactId = 1;
		var queryResult = new CommandResult(status, message: message);
		var expected = queryResult.ConvertToActionResult();

		A.CallTo(() => _sender.Send(A<GetContactByIdQuery>.That.Matches(query => query.Id == contactId), A<CancellationToken>.Ignored))
			.Returns(queryResult);

		// Act
		var result = await _contactController.GetContact(contactId);

		// Assert
		result.Result.Should().BeOfType(expected.GetType());
		if (result.Result is ObjectResult objResult)
			objResult.Value.Should().BeEquivalentTo(message);
	}

	#endregion

	#region Post.Tests

	[Fact]
	public async Task PostContact_ReturnsCreatedAtWithContact_WhenSuccessResultWithContact()
	{
		// Arrange
		var fakeContact = new Contact
		{
			Id = 1,
			TelephoneNumber = "Test"
		};
		var fakeResult = new CommandResult(CommandStatus.Success, value: fakeContact);
		var command = new CreateContactCommand("Test", "2000@mm.c", "TestGenre", 2);
		A.CallTo(() => _sender.Send(command, A<CancellationToken>.Ignored))
			.Returns(fakeResult);

		// Act
		var result = await _contactController.PostContact(command);

		// Assert
		var createdResult = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
		createdResult!.Value.Should().BeEquivalentTo(fakeContact);
	}

	[Theory]
	[InlineData(CommandStatus.NotFound)]
	[InlineData(CommandStatus.BadRequest)]
	[InlineData((CommandStatus)999)]
	public async Task PostContact_ReturnsBadAction_WhenStatusIsNotSuccess(CommandStatus status)
	{
		// Arrange
		var errorMessage = "SomeMessage";
		var fakeResult = new CommandResult(status, message: errorMessage);
		var command = new CreateContactCommand("Test", "2000@mm.c", "TestGenre", 2);
		var expected = fakeResult.ConvertToActionResult();

		A.CallTo(() => _sender.Send(command, A<CancellationToken>.Ignored))
			.Returns(fakeResult);

		// Act
		var result = await _contactController.PostContact(command);

		// Assert
		result.Result.Should().BeOfType(expected.GetType());
		if (result.Result is ObjectResult objResult)
			objResult.Value.Should().BeEquivalentTo(errorMessage);
	}

	#endregion

	#region Put.Tests

	[Fact]
	public async Task PutContact_ReturnsNoContentResult_WhenResultStatusSuccess()
	{
		// Arrange
		var resultStatus = new CommandResult(CommandStatus.Success);
		var command = new UpdateContactCommand(1, "Test", "2000@mm.c", "TestGenre", 2);
		A.CallTo(() => _sender.Send(command, A<CancellationToken>.Ignored))
			.Returns(resultStatus);

		// Act
		var result = await _contactController.PutContact(command);

		// Assert
		result.Should().BeOfType(typeof(NoContentResult));
	}

	[Theory]
	[InlineData(CommandStatus.NotFound)]
	[InlineData(CommandStatus.BadRequest)]
	[InlineData((CommandStatus)999)]
	public async Task PutContact_ReturnsBadResult_WhenBadCommandStatus(CommandStatus status)
	{
		// Arrange
		var message = "Test";
		var resultStatus = new CommandResult(status, message: message);
		var command = new UpdateContactCommand(1, "Test", "2000@mm.c", "TestGenre", 2);
		var expected = resultStatus.ConvertToActionResult();
		A.CallTo(() => _sender.Send(command, A<CancellationToken>.Ignored))
			.Returns(resultStatus);

		// Act
		var result = await _contactController.PutContact(command);

		// Assert
		result.Should().BeOfType(expected.GetType());
		if (result is ObjectResult objResult)
			objResult.Value.Should().BeEquivalentTo(message);
	}

	#endregion

	#region Delete.Tests

	[Fact]
	public async Task DeleteContact_ReturnsOkWithContact_WhenCommandStatusSuccess()
	{
		// Arrange
		int fakeId = 4;
		Contact? fakeContact = new Contact
		{
			Id = 7,
			TelephoneNumber = "Test_Number"
		};

		var fakeResult = new CommandResult(CommandStatus.Success, value: fakeContact);
		A.CallTo(() => _sender.Send(A<DeleteContactByIdCommand>
			.That.Matches(command => command.Id == fakeId), A<CancellationToken>.Ignored))
			.Returns(fakeResult);

		// Act
		var result = await _contactController.DeleteContact(fakeId);

		// Assert
		result.Result.Should().BeOfType(typeof(OkObjectResult));
		var okObject = result.Result as OkObjectResult;
		okObject!.Value.Should().Be(fakeContact);
	}


	[Theory]
	[InlineData(CommandStatus.NotFound)]
	[InlineData(CommandStatus.BadRequest)]
	[InlineData((CommandStatus)999)]
	public async Task DeleteContact_ReturnsBadResult_WhenStatusNotSuccess(CommandStatus status)
	{
		// Arrange
		int fakeId = 4;
		var message = "Test";
		var fakeResult = new CommandResult(status, message: message);
		var expected = fakeResult.ConvertToActionResult();
		A.CallTo(() => _sender.Send(A<DeleteContactByIdCommand>
			.That.Matches(command => command.Id == fakeId), A<CancellationToken>.Ignored))
			.Returns(fakeResult);

		// Act
		var result = await _contactController.DeleteContact(fakeId);

		// Assert
		result.Result.Should().BeOfType(expected.GetType());
		if (result.Result is ObjectResult objResult)
			objResult.Value.Should().BeEquivalentTo(message);
	}

	#endregion
}
