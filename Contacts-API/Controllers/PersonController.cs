using ContactsAPI.Application.Features;
using ContactsAPI.Application.Features.AllPerson.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContactsAPI.Controllers;

[ApiController]
[Route("api/people")]
public class PersonController : ControllerBase
{
	private readonly ISender _sender;
	private readonly CancellationTokenSource _cts;

	public PersonController(ISender sender)
	{
		_sender = sender;
		_cts = new CancellationTokenSource();
	}

	// GET api/people
	[HttpGet]
	public async Task<ActionResult<IEnumerable<object>>> GetAllPerson()
	{
		var result = await _sender.Send(new GetAllPersonQuery(), _cts.Token);
		if (result.Status == CommandStatus.Success)
			return Ok(result.Value);

		return result.ConvertToActionResult();
	}


	// GET /api/people/{id}
	[HttpGet("{id:int}")]
	public async Task<ActionResult<object>> GetPerson(int id)
	{
		var getResult = await _sender.Send(new GetPersonByIdQuery(id), _cts.Token);

		if (getResult.Status == CommandStatus.Success)
			return Ok(getResult.Value);

		return getResult.ConvertToActionResult();
	}

	// POST api/people
	[HttpPost]
	public async Task<ActionResult<object>> PostPerson(CreatePersonCommand command)
	{
		var createResult = await _sender.Send(command, _cts.Token);

		if (createResult.Status == CommandStatus.Success)
			return CreatedAtAction(nameof(GetAllPerson), createResult.Value);

		return createResult.ConvertToActionResult();
	}

	// PUT api/people/{id}
	[HttpPut]
	public async Task<ActionResult> PutPerson(UpdatePersonCommand command)
	{
		var putResult = await _sender.Send(command, _cts.Token);

		if (putResult.Status == CommandStatus.Success)
			return NoContent();

		return putResult.ConvertToActionResult();
	}

	// DELETE api/people/{id}
	[HttpDelete("{id:int}")]
	public async Task<ActionResult<object>> DeletePerson(int id)
	{
		var command = new DeletePersonByIdCommand(id);
		var deleteResult = await _sender.Send(command, _cts.Token);

		if (deleteResult.Status == CommandStatus.Success)
			return Ok(deleteResult.Value);

		return deleteResult.ConvertToActionResult();
	}
}
