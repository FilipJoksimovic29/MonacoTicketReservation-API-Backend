using Microsoft.AspNetCore.Mvc;
using RezervacijaKarataAPI.DTO;
using RezervacijaKarata.Services.Interfaces;
using System.Threading.Tasks;
using RezervacijaKarata.Domain.Model;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCustomers()
    {
        var customers = await _customerService.GetAllCustomersAsync();
        var customersDto = customers.Select(CustomerDTOMapper.ToDTO).ToList();
        return Ok(customersDto);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomerById(int id)
    {
        var customer = await _customerService.GetCustomerByIdAsync(id);
        if (customer == null)
            return NotFound();
        return Ok(CustomerDTOMapper.ToDTO(customer));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerDTO customerDto)
    {
        // Explicitly ignore the Booking field when mapping for creation
        var customer = new Customer
        {
            Id = customerDto.Id,
            FirstName = customerDto.FirstName,
            LastName = customerDto.LastName,
            Company = customerDto.Company,
            Address1 = customerDto.Address1,
            Address2 = customerDto.Address2,
            PostalCode = customerDto.PostalCode,
            City = customerDto.City,
            Country = customerDto.Country,
            Email = customerDto.Email
        };

        var createdCustomer = await _customerService.CreateCustomerAsync(customer);
        var customerToReturn = CustomerDTOMapper.ToDTO(createdCustomer);
        customerToReturn.Booking = null; // Ensure no Booking is attached during creation
        return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomer.Id }, customerToReturn);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] CustomerDTO customerDto)
    {
        var customer = CustomerDTOMapper.ToCustomer(customerDto);
        customer.Id = id; // Ensure correct ID is set
        await _customerService.UpdateCustomerAsync(customer);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        await _customerService.DeleteCustomerAsync(id);
        return NoContent();
    }
}
