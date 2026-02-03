namespace İlkProjem.backend.Services;

using İlkProjem.backend.Dtos.CustomerDtos;
using İlkProjem.backend.Data;
using İlkProjem.backend.Models;
using Microsoft.EntityFrameworkCore;

public class CustomerService
{
    private readonly AppDbContext _context;

    public CustomerService(AppDbContext context)
    {
        _context = context;
    }

    // "Ekle" - CreateDto alıp Customer modeline çevirir
    public async Task AddCustomer(CustomerCreateDto createDto)
    {
        var customer = new Customer 
        {
            Name = createDto.Name,
            Email = createDto.Email
            // Id ve CreatedAt veritabanı tarafından halledilecek
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
    }

    // "Getir" - Tüm listeyi ReadDto olarak döndürür
    public async Task<List<CustomerReadDto>> GetAllCustomers()
    {
        return await _context.Customers
            .Select(c => new CustomerReadDto 
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                CreatedAt = c.CreatedAt
            })
            .ToListAsync();
    }

    // "GetirById" - Tek bir ReadDto döndürür
    public async Task<CustomerReadDto?> GetCustomerById(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        
        if (customer == null) return null;

        return new CustomerReadDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            CreatedAt = customer.CreatedAt
        };
    }

    // "Güncelle" - UpdateDto kullanır
    public async Task<bool> UpdateCustomer(CustomerUpdateDto updateDto)
    {
        var existingCustomer = await _context.Customers.FindAsync(updateDto.Id);
        
        if (existingCustomer == null) return false;

        existingCustomer.Name = updateDto.Name;
        existingCustomer.Email = updateDto.Email;

        await _context.SaveChangesAsync();
        return true;
    }

    // "Sil" - DeleteDto alıp işlemi yapar
    public async Task<bool> DeleteCustomer(CustomerDeleteDto deleteDto)
    {
        var customer = await _context.Customers.FindAsync(deleteDto.Id);
        
        if (customer == null) return false;

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return true;
    }
}