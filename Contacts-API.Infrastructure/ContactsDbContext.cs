using ContactsAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ContactsAPI.Infrastructure;

public class ContactsDbContext : DbContext
{
	private readonly IConfiguration _configuration;
	public DbSet<Contact> Contacts { get; set; }
	public DbSet<Person> AllPerson { get; set; }

	public ContactsDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
	{
		_configuration = configuration;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		if (!optionsBuilder.IsConfigured)
		{
			optionsBuilder.UseNpgsql(_configuration.GetConnectionString("ContactsDb"));
			base.OnConfiguring(optionsBuilder);
		}
	}
}
