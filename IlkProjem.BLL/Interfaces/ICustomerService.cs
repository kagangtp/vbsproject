using IlkProjem.Core.Dtos.CustomerDtos;
using IlkProjem.Core.Utilities.Results;

namespace IlkProjem.BLL.Interfaces;

public interface ICustomerService
{
    Task<IResult> AddCustomer(CustomerCreateDto createDto);
    Task<IDataResult<List<CustomerReadDto>>> GetAllCustomers();
    Task<IDataResult<CustomerReadDto>> GetCustomerById(int id);
    Task<IResult> UpdateCustomer(CustomerUpdateDto updateDto);
    Task<IResult> DeleteCustomer(CustomerDeleteDto deleteDto);
}