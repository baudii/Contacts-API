using ContactsAPI.Domain.Models;
using ContactsAPI.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Tests;

public static class TestUtils
{
	public static async Task<ContactsDbContext> GetTemporaryDbContextAsync()
	{
		var options = new DbContextOptionsBuilder<ContactsDbContext>()
			.UseInMemoryDatabase(databaseName: $"TestDatabase{Guid.NewGuid()}")
			.Options;

		var context = new ContactsDbContext(options);

		await context.Database.EnsureCreatedAsync();


		if (await context.AllPerson.CountAsync() <= 0)
		{
			for (var i = 1; i < 10; i++)
			{
				var person = GetStandardPerson();
				context.AllPerson.Add(person);
			}
			await context.SaveChangesAsync();
		}

		if (await context.Contacts.CountAsync() <= 0)
		{
			for (var i = 1; i < 10; i++)
			{
				var contact = GetStandardContact();
				contact.PersonId = i;
				context.Contacts.Add(contact);
			}
			await context.SaveChangesAsync();
		}

		return context;
	}

	public static Contact GetStandardContact()
	{
		return new Contact
		{
			TelephoneNumber = "Test",
			Email = "sss@ss.ss",
			LinkedIn = "Test"
		};
	}

	public static Person GetStandardPerson()
	{
		return new Person
		{
			FullName = "Test Name",
			Birthdate = new DateOnly(1999, 12, 12)
		};
	}

	public static object? GetAnonymousProperty(this object obj, string propName)
	{
		var type = obj.GetType();
		var prop = type.GetProperty(propName);
		object? val = prop?.GetValue(obj);
		return val;
	}
}
