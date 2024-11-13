using ContactsAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Infrastructure;

public class ContactsDbContext : DbContext
{
	public DbSet<Contact> Contacts { get; set; }
	public DbSet<Person> AllPerson { get; set; }

	public ContactsDbContext(DbContextOptions options) : base(options)
	{
	}
}
