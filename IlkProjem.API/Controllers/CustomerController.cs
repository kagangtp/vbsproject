using Microsoft.AspNetCore.Mvc;
using IlkProjem.Core.Dtos.CustomerDtos;
using IlkProjem.BLL.Interfaces;
using IlkProjem.Core.Models;
using IlkProjem.Core.Dtos.SpecificationDtos;
using Microsoft.AspNetCore.Authorization;

namespace IlkProjem.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // "Getir" - Tüm Liste
    // [HttpGet] 
    // public async Task<IActionResult> Get() 
    // {
    //     var result = await _customerService.GetAllCustomers();
    //     return result.Success ? Ok(result) : BadRequest(result);
    // }

    [HttpGet]
    public async Task<IActionResult> GetCustomers([FromQuery] CustomerSpecParams custParams,CancellationToken ct)
    {
        var result = await _customerService.GetCustomersAsync(custParams, ct);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // "GetirById"
    [HttpGet("{id}")] 
    public async Task<IActionResult> Get(int id)
    {
        var result = await _customerService.GetCustomerById(id);
        return result.Success ? Ok(result) : NotFound(result);
    }

    // "Ekle"
    [HttpPost] 
    public async Task<IActionResult> Post(CustomerCreateDto createDto)
    {
        var result = await _customerService.AddCustomer(createDto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    // "Güncelle"
    [HttpPut] 
    public async Task<IActionResult> Update(CustomerUpdateDto updateDto)
    {
        var result = await _customerService.UpdateCustomer(updateDto);
        return result.Success ? Ok(result) : NotFound(result);
    }

    // "Sil"
    [HttpDelete] 
    public async Task<IActionResult> Delete(CustomerDeleteDto deleteDto)
    {
        var result = await _customerService.DeleteCustomer(deleteDto);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}