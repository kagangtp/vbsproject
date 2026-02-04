// Interfaces/ICustomerRepository.cs
using İlkProjem.backend.Models;

namespace İlkProjem.backend.Interfaces;
public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task AddAsync(Customer customer);
    Task<bool> UpdateAsync(Customer customer);
    Task<bool> DeleteAsync(int id);
}