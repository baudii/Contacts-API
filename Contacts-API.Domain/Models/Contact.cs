namespace ContactsAPI.Domain.Models;

public class Contact
{
	public int Id { get; set; }
	public string TelephoneNumber { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string LinkedIn { get; set; } = string.Empty;
	public int PersonId { get; set; }

	public Person? Person { get; set; }
}
