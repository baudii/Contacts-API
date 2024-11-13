using ContactsAPI.Application.Interfaces;
using ContactsAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Infrastructure.Repositories;

public class ContactRepository : ICrudRepository<Contact>
{
	private readonly ContactsDbContext _context;
	public ContactRepository(ContactsDbContext context)
	{
		_context = context;
	}
	public async Task<IEnumerable<Contact>> GetAllAsync(CancellationToken token)
	{
		return await _context.Contacts.ToListAsync(token);
	}

	public async Task<Contact?> GetByIdAsync(int id, CancellationToken token)
	{
		return await _context.Contacts.FindAsync(id, token);
	}

	public async Task<Contact> AddAsync(Contact contact, CancellationToken token)
	{
		var result = await _context.Contacts.AddAsync(contact, token);
		await _context.SaveChangesAsync(token);
		return result.Entity;
	}

	public async Task UpdateAsync(Contact contact, CancellationToken token)
	{
		_context.Contacts.Update(contact);
		await _context.SaveChangesAsync(token);
	}

	public async Task<Contact?> DeleteAsync(int id, CancellationToken token)
	{
		var contact = await _context.Contacts.FindAsync(id, token);
		if (contact != null)
		{
			_context.Contacts.Remove(contact);
			await _context.SaveChangesAsync(token);
		}
		return contact;
	}
}
