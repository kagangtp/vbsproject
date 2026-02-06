using İlkProjem.backend.Dtos.CustomerDtos;
using İlkProjem.backend.Interfaces;
using İlkProjem.backend.Models;

namespace İlkProjem.backend.Services;

public class CustomerService
{
    private readonly ICustomerRepository _repository; // Artık context değil, repository var

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    // "Ekle" - CreateDto -> Entity
    public async Task AddCustomer(CustomerCreateDto createDto)
    {
        var customer = new Customer 
        {
            Name = createDto.Name,
            Email = createDto.Email
        };

        await _repository.AddAsync(customer);
    }

    // "Getir" - Entity Listesi -> ReadDto Listesi
    public async Task<List<CustomerReadDto>> GetAllCustomers()
    {
        var customers = await _repository.GetAllAsync();
        
        return customers.Select(c => new CustomerReadDto 
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Balance = c.Balance,
            CreatedAt = c.CreatedAt
        }).ToList();
    }

    // "GetirById" - Entity -> ReadDto
    public async Task<CustomerReadDto?> GetCustomerById(int id)
    {
        var customer = await _repository.GetByIdAsync(id);
        
        if (customer == null) return null;

        return new CustomerReadDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            CreatedAt = customer.CreatedAt
        };
    }

    // "Güncelle" - UpdateDto -> Entity
    public async Task<bool> UpdateCustomer(CustomerUpdateDto updateDto)
    {
        // Önce veritabanından mevcut kaydı buluyoruz
        var existingCustomer = await _repository.GetByIdAsync(updateDto.Id);
        if (existingCustomer == null) return false;

        // Bilgileri güncelliyoruz
        existingCustomer.Name = updateDto.Name;
        existingCustomer.Email = updateDto.Email;

        // FIX::Solid NO IRL: A mistake to include Balance in the update DTO, but we will ignore this for now
        existingCustomer.Balance = updateDto.Balance;

        return await _repository.UpdateAsync(existingCustomer);
    }

    // "Sil" - DeleteDto ID -> Delete
    public async Task<bool> DeleteCustomer(CustomerDeleteDto deleteDto)
    {
        return await _repository.DeleteAsync(deleteDto.Id);
    }

   
}