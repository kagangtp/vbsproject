using IlkProjem.DAL.Repositories;
using IlkProjem.Core.Models;
using IlkProjem.Core.Dtos.CustomerDtos;
using IlkProjem.BLL.Interfaces;
using IlkProjem.Core.Utilities.Results; // Ensure this matches your namespace
using Microsoft.Extensions.Localization;
using IlkProjem.Core.Resources; // Pointing to Core now

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

    public async Task<IDataResult<List<CustomerReadDto>>> GetAllCustomers()
    {
        var customers = await _repository.GetAllAsync();
        var data = customers.Select(c => new CustomerReadDto { /* mapping */ }).ToList();

        return new SuccessDataResult<List<CustomerReadDto>>(data, _localizer["CustomersListed"]);
    }

    public async Task<IDataResult<CustomerReadDto>> GetCustomerById(int id)
    {
        var customer = await _repository.GetByIdAsync(id);
        if (customer == null) 
            return new ErrorDataResult<CustomerReadDto>(_localizer["CustomerNotFound"]);

        var data = new CustomerReadDto { /* mapping */ };
        return new SuccessDataResult<CustomerReadDto>(data);
    }

    public async Task<IResult> UpdateCustomer(CustomerUpdateDto updateDto)
    {
        var existingCustomer = await _repository.GetByIdAsync(updateDto.Id);
        if (existingCustomer == null) 
            return new ErrorResult(_localizer["CustomerNotFound"]);

        // ... update logic ...
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