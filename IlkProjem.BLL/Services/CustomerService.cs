using IlkProjem.DAL.Repositories;
using IlkProjem.Core.Models;
using IlkProjem.Core.Dtos.CustomerDtos;
using IlkProjem.BLL.Interfaces;
using IlkProjem.Core.Utilities.Results;
using Microsoft.Extensions.Localization;
using IlkProjem.Core.Resources;
using IlkProjem.Core.Dtos.SpecificationDtos;

namespace IlkProjem.BLL.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IStringLocalizer<Messages> _localizer;

    public CustomerService(ICustomerRepository repository, IStringLocalizer<Messages> localizer)
    {
        _repository = repository;
        _localizer = localizer;
    }

    public async Task<IResult> AddCustomer(CustomerCreateDto createDto)
    {
        var customer = new Customer { Name = createDto.Name, Email = createDto.Email };
        await _repository.AddAsync(customer);
        
        // Uses key from Core/Resources/Messages.resx
        return new SuccessResult(_localizer["CustomerAdded"]); 
    }

    public async Task<IDataResult<List<CustomerReadDto>>> GetCustomersAsync(CustomerSpecParams custParams, CancellationToken ct = default)
    {
        // 1. Create the Spec with the incoming params
        var spec = new CustomerCursorSpecification(custParams);

        // 2. The Repository uses the Spec to build the optimized IQueryable
        var customers = await _repository.ListAsync(spec, ct);

        // 3. Map entities to DTOs (same thing you do in GetCustomerById, but for a list)
        var customerDtos = customers.Select(c => new CustomerReadDto
        {
            Id = c.Id,
            Name = c.Name,
            Email = c.Email,
            Balance = c.Balance,
            CreatedAt = c.CreatedAt
        }).ToList();

        // 4. Wrap in result and return
        return new SuccessDataResult<List<CustomerReadDto>>(customerDtos, "Customers retrieved");
    }

    public async Task<IDataResult<CustomerReadDto>> GetCustomerById(int id)
    {
        var customer = await _repository.GetByIdAsync(id);
        if (customer == null) 
            return new ErrorDataResult<CustomerReadDto>(_localizer["CustomerNotFound"]);

        var data = new CustomerReadDto { 
            Id = customer.Id, 
            Name = customer.Name, 
            Email = customer.Email, 
            Balance = customer.Balance,
            CreatedAt = customer.CreatedAt
        };
        return new SuccessDataResult<CustomerReadDto>(data);
    }

    public async Task<IResult> UpdateCustomer(CustomerUpdateDto updateDto)
    {
        var existingCustomer = await _repository.GetByIdAsync(updateDto.Id);
        if (existingCustomer == null) 
            return new ErrorResult(_localizer["CustomerNotFound"]);
        
        existingCustomer.Name = updateDto.Name;
        existingCustomer.Email = updateDto.Email;
        existingCustomer.Balance = updateDto.Balance;

        await _repository.UpdateAsync(existingCustomer);
        return new SuccessResult(_localizer["CustomerUpdated"]);
    }

    public async Task<IResult> DeleteCustomer(CustomerDeleteDto deleteDto)
    {
        var deleted = await _repository.DeleteAsync(deleteDto.Id);
        if (!deleted) return new ErrorResult(_localizer["DeleteError"]);

        return new SuccessResult(_localizer["CustomerDeleted"]);
    }
}