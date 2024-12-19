using BankingAPI.Application.DTOs;
using BankingAPI.Application.Requests;
using BankingAPI.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankingAPI.API.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomerController(ICustomerService customerService,IJwtService jwtService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest createCustomerRequest)
    {
        var customerDto = await customerService.RegisterCustomerAsync(createCustomerRequest);
        return CreatedAtAction(nameof(GetCustomerById), new { id = customerDto.Id }, customerDto);
    }


    [HttpPost("token")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { Message = "Invalid input." });

        var tokenDto = await customerService.AuthenticateCustomerAsync(loginRequest);

        return tokenDto == null
            ? Unauthorized(new { Message = "Invalid email or password." })
            : Ok(tokenDto);
    }


    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers()
    {
        var customers = await customerService.RetrieveAllCustomersAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(Guid id)
    {
        var customerDto = await customerService.GetCustomerDetailsByIdAsync(id);
        if (customerDto == null) return NotFound(new { message = "Customer not found." });

        return Ok(customerDto);
    }


    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateCustomer(Guid id, [FromBody] UpdateCustomerRequest updateCustomerRequest)
    {
        await customerService.UpdateCustomerDetailsAsync(id, updateCustomerRequest);
        return Ok(new { message = "Customer updated successfully." });
    }

    [HttpDelete("hard/{id}")]
    [Authorize]
    public async Task<IActionResult> HardDeleteCustomer(Guid id)
    {
        await customerService.PermanentlyDeleteCustomerAsync(id);
        return NoContent();
    }



    [HttpDelete("soft/{id}")]
    [Authorize]
    public async Task<IActionResult> SoftDeleteCustomer(Guid id)
    {
        await customerService.DeactivateCustomerAsync(id);
        return NoContent();
    }

    [HttpPatch("{id}/password")]
    [Authorize]
    public async Task<IActionResult> UpdatePassword(Guid id,
        [FromBody] CustomerPasswordChangeRequest customerPasswordChangeRequest)
    {
        // Pass the URL `id` to the service method
        var isUpdated = await customerService.ChangeCustomerPasswordAsync(id, customerPasswordChangeRequest);

        if (!isUpdated)
            return BadRequest(new { message = "Current password is incorrect or new password is invalid." });

        return Ok(new { message = "Password updated successfully." });
    }


    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout([FromBody] string token)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            return BadRequest("Token is required.");
        }

        await customerService.LogoutCustomerAsync(token);
    
        return Ok("Logout successful.");
    }


}