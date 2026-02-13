// Interfaces/ICustomerRepository.cs
using IlkProjem.Core.Models;
using IlkProjem.Core.Specifications;

namespace IlkProjem.DAL.Repositories;
public interface ICustomerRepository
{
    Task<List<Customer>> GetAllAsync();
    Task<Customer?> GetByIdAsync(int id);
    Task AddAsync(Customer customer);
    Task<bool> UpdateAsync(Customer customer);
    Task<bool> DeleteAsync(int id);

    // NEW: Accept a Specification and return only the matching customers
    Task<List<Customer>> ListAsync(ISpecification<Customer> spec);
}