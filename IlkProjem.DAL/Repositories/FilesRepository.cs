using IlkProjem.DAL.Repositories;
using IlkProjem.Core.Models;
using IlkProjem.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace IlkProjem.DAL.Repositories;

public class FilesRepository : IFilesRepository
{
    private readonly AppDbContext _context;

    public FilesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Files file)
    {
        await _context.Files.AddAsync(file);
    }

    public async Task<Files?> GetByIdAsync(Guid id)
    {
        return await _context.Files.FindAsync(id);
    }

    public async Task<List<Files>> GetAllAsync()
    {
        return await _context.Files.ToListAsync();
    }

    public void Delete(Files file)
    {
        _context.Files.Remove(file);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}