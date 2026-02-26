using IlkProjem.Core.Models;

namespace IlkProjem.DAL.Repositories;

public interface IFilesRepository
{
    Task AddAsync(Files file);
    Task<Files?> GetByIdAsync(Guid id);
    Task<List<Files>> GetAllAsync();
    Task<List<Files>> GetByOwnerAsync(string ownerType, int ownerId);
    void Update(Files file);
    void Delete(Files file);
    Task SaveChangesAsync();
}