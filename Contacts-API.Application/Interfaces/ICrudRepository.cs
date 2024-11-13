namespace ContactsAPI.Application.Interfaces;

public interface ICrudRepository<T>
{
	Task<IEnumerable<T>> GetAllAsync(CancellationToken token);
	Task<T?> GetByIdAsync(int id, CancellationToken token);
	Task<T> AddAsync(T contact, CancellationToken token);
	Task UpdateAsync(T contact, CancellationToken token);
	Task<T?> DeleteAsync(int id, CancellationToken token);
}