// Interfaces/ICustomerRepository.cs
using IlkProjem.Core.Models;

namespace IlkProjem.DAL.Repositories;
public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task AddAsync(Customer customer);
    Task<bool> UpdateAsync(Customer customer);
    Task<bool> DeleteAsync(int id);
}