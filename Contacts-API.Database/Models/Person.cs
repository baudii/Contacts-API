namespace ContactsAPI.Domain.Models;

public class Person
{
	public int Id { get; set; }
	public string FullName { get; set; } = string.Empty;
	public DateOnly Birthdate { get; set; }
}
