using ContactsAPI.Application.Interfaces;
using ContactsAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Infrastructure.Repositories;

public class PersonRepository : ICrudRepository<Person>
{
	private readonly ContactsDbContext _context;
	public PersonRepository(ContactsDbContext context)
	{
		_context = context;
	}

	public async Task<IEnumerable<Person>> GetAllAsync(CancellationToken token)
	{
		return await _context.AllPerson.ToListAsync(token);
	}

	public async Task<Person?> GetByIdAsync(int id, CancellationToken token)
	{
		return await _context.AllPerson.FindAsync(id, token);
	}

	public async Task<Person> AddAsync(Person person, CancellationToken token)
	{
		var result = await _context.AllPerson.AddAsync(person, token);
		await _context.SaveChangesAsync(token);
		return result.Entity;
	}

	public async Task UpdateAsync(Person person, CancellationToken token)
	{
		_context.AllPerson.Update(person);
		await _context.SaveChangesAsync(token);
	}

	public async Task<Person?> DeleteAsync(int id, CancellationToken token)
	{
		var person = await _context.AllPerson.FindAsync(id, token);
		if (person != null)
		{
			_context.AllPerson.Remove(person);
			await _context.SaveChangesAsync(token);
		}
		return person;
	}
}
