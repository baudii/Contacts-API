namespace ContactsAPI.Application.Features;

public struct CommandResult
{
	public CommandStatus Status;
	public string? Message;
	public object? Value;

	public CommandResult(CommandStatus resultType, object? value = null, string? message = null)
	{
		Message = message;
		Value = value;
		Status = resultType;
	}
}

public enum CommandStatus
{
	Success,
	BadRequest,
	NotFound,
	Internal500
}
