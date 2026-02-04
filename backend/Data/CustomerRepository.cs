using İlkProjem.backend.Interfaces;
using İlkProjem.backend.Models;
using Microsoft.EntityFrameworkCore;

namespace İlkProjem.backend.Data;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Customer>> GetAllAsync() => await _context.Customers.ToListAsync();

    public async Task<Customer?> GetByIdAsync(int id) => await _context.Customers.FindAsync(id);

    public async Task AddAsync(Customer customer)
    {
        await _context.Customers.AddAsync(customer);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(Customer customer)
    {
        _context.Customers.Update(customer);
        
        // Değişiklikleri kaydet ve kaç satırın etkilendiğini al
        int affectedRows = await _context.SaveChangesAsync();
        
        // Eğer en az 1 satır güncellendiyse true, aksi halde false döner
        return affectedRows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        // 1. Önce veritabanından o ID'ye sahip kaydı bulmamız lazım
        var customer = await _context.Customers.FindAsync(id);
        
        // 2. Eğer öyle bir müşteri yoksa silme işlemine gerek yok, false dön
        if (customer == null) return false;

        // 3. Müşteri bulunduysa şimdi silebiliriz
        _context.Customers.Remove(customer);
        
        int affectedRows = await _context.SaveChangesAsync();
        return affectedRows > 0;
    }
}