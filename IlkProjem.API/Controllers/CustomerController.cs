using Microsoft.AspNetCore.Mvc;
using IlkProjem.BLL.Services;
using IlkProjem.Core.Dtos.CustomerDtos;

namespace IlkProjem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CustomerService _service;

    public CustomerController(CustomerService service) => _service = service;

    // "Getir" Butonu (Tüm Liste)
    [HttpGet] 
    public async Task<ActionResult<List<CustomerReadDto>>> Get() 
        => Ok(await _service.GetAllCustomers()); // Servis artık ReadDto dönmeli

    // "GetirById" Butonu
    [HttpGet("{id}")] 
    public async Task<ActionResult<CustomerReadDto>> Get(int id)
    {
        var customer = await _service.GetCustomerById(id);
        return customer == null ? NotFound() : Ok(customer);
    }

    // "Güncelle" Butonu
    [HttpPut] 
    public async Task<IActionResult> Update(CustomerUpdateDto updateDto)
    {
        var success = await _service.UpdateCustomer(updateDto);
        return success ? Ok("Güncellendi") : NotFound("Müşteri bulunamadı");
    }

    // "Ekle" Butonu
    [HttpPost] 
    public async Task<IActionResult> Post(CustomerCreateDto createDto)
    {
        await _service.AddCustomer(createDto); // Artık CreateDto gönderiyoruz
        return Ok("Müşteri eklendi");
    }

    // "Sil" Butonu
    [HttpDelete] 
    public async Task<IActionResult> Delete(CustomerDeleteDto deleteDto)
    {
        var success = await _service.DeleteCustomer(deleteDto);
        return success ? NoContent() : NotFound("Silinecek müşteri bulunamadı");
    }
}