using IlkProjem.Core.Models;
using IlkProjem.Core.Specifications;
using IlkProjem.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace IlkProjem.DAL.Repositories;

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

    // NEW: This is where IQueryable + Specification come together
    public async Task<List<Customer>> ListAsync(ISpecification<Customer> spec)
    {
        // Step 1: Start with ALL customers as an IQueryable (no SQL yet!)
        var query = _context.Customers.AsQueryable();

        // Step 2: Let the SpecificationEvaluator apply the filters/sorting/paging
        //         This STILL doesn't execute SQL — just builds the query
        query = SpecificationEvaluator<Customer>.GetQuery(query, spec);

        // Step 3: NOW execute the SQL and get the results
        return await query.ToListAsync();
    }
}