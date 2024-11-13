using ContactsAPI.Application.Features.AllPerson.Commands;
using ContactsAPI.Application.Features;
using ContactsAPI.Controllers;
using ContactsAPI.Domain.Models;
using FakeItEasy;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContactsAPI.Tests.ControllersTests;

public class PersonControllerTests
{
	private readonly ISender _sender;
	private readonly PersonController _personController;

	public PersonControllerTests()
	{
		// Dependancies
		_sender = A.Fake<ISender>();

		// SUT (System Under Test)
		_personController = new PersonController(_sender);
	}

	[Fact]
	public async void GetAllPerson_ReturnsOkObject_WhenAllPersonExists()
	{
		// Arrange
		var allPerson = new List<Person>
		{
			new Person { Id = 1, FullName = "Test Name 1", Birthdate = DateOnly.MinValue },
			new Person { Id = 2, FullName = "Test Name 2", Birthdate = DateOnly.MinValue },
			new Person { Id = 3, FullName = "Test Name 3", Birthdate = DateOnly.MinValue }
		};
		var getAllPersonResult = new CommandResult(CommandStatus.Success, allPerson);
		A.CallTo(() => _sender.Send(A<GetAllPersonQuery>.Ignored, A<CancellationToken>.Ignored)
			).Returns(getAllPersonResult);

		// Act
		var result = await _personController.GetAllPerson();

		// Assert
		result.Result.Should().BeOfType(typeof(OkObjectResult));
		var okObjResult = result.Result as OkObjectResult;
		okObjResult!.Value.Should().BeEquivalentTo(allPerson);
	}

	[Theory]
	[InlineData(CommandStatus.NotFound)]
	[InlineData(CommandStatus.BadRequest)]
	[InlineData((CommandStatus)999)]
	public async void GetAllPerson_ReturnsCorrectActionResult_WhenResultStatusIsNotSuccess(CommandStatus status)
	{
		// Arrange
		var getAllPersonResult = new CommandResult(status);
		ActionResult? expected = getAllPersonResult.ConvertToActionResult();
		A.CallTo(() => _sender.Send(A<GetAllPersonQuery>.Ignored, A<CancellationToken>.Ignored))!
			.Returns(getAllPersonResult);

		// Act
		var result = await _personController.GetAllPerson();

		// Assert
		result.Result.Should().BeOfType(expected.GetType());
	}
}
