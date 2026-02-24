using IlkProjem.Core.Models;

namespace IlkProjem.DAL.Repositories;

public interface IFilesRepository
{
    Task AddAsync(Files file);
    Task<Files?> GetByIdAsync(Guid id);
    Task<List<Files>> GetAllAsync();
    void Delete(Files file);
    Task SaveChangesAsync();
}