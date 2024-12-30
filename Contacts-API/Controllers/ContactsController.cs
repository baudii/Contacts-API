using ContactsAPI.Application.Features;
using ContactsAPI.Application.Features.Contacts.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ContactsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactsController : ControllerBase
{
	private readonly ISender _sender;
	private readonly CancellationTokenSource _cts;

	public ContactsController(ISender sender)
	{
		_sender = sender;
		_cts = new CancellationTokenSource();
	}

	// GET api/contacts
	[HttpGet]
	public async Task<ActionResult<IEnumerable<object>>> GetContacts()
	{
		var getResult = await _sender.Send(new GetContactsQuery());

		if (getResult.Status == CommandStatus.Success)
			return Ok(getResult.Value);

		return getResult.ConvertToActionResult();
	}

	// GET /api/contacts/{id}
	[HttpGet("{id:int}")]
	public async Task<ActionResult<object>> GetContact(int id)
	{
		var getResult = await _sender.Send(new GetContactByIdQuery(id), _cts.Token);

		if (getResult.Status == CommandStatus.Success)
			return Ok(getResult.Value);

		return getResult.ConvertToActionResult();
	}

	// POST api/contacts
	[HttpPost]
	public async Task<ActionResult<object>> PostContact(CreateContactCommand command)
	{
		var createResult = await _sender.Send(command, _cts.Token);

		if (createResult.Status == CommandStatus.Success)
			return CreatedAtAction(nameof(GetContacts), createResult.Value);

		return createResult.ConvertToActionResult();
	}

	// PUT api/contacts/{id}
	[HttpPut]
	public async Task<ActionResult> PutContact(UpdateContactCommand command)
	{
		var putResult = await _sender.Send(command, _cts.Token);

		if (putResult.Status == CommandStatus.Success)
			return NoContent();

		return putResult.ConvertToActionResult();
	}

	// DELETE api/contacts/{id}
	[HttpDelete("{id:int}")]
	public async Task<ActionResult<object>> DeleteContact(int id)
	{
		var command = new DeleteContactByIdCommand(id);
		var deleteResult = await _sender.Send(command, _cts.Token);

		if (deleteResult.Status == CommandStatus.Success)
			return Ok(deleteResult.Value);

		return deleteResult.ConvertToActionResult();
	}
}
