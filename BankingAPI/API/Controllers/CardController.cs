using BankingAPI.Application.DTOs;
using BankingAPI.Application.Requests;
using BankingAPI.Domain.Interfaces;
using BankingAPI.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace BankingAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardsController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpPost]
        public async Task<ActionResult<CardDto>> CreateCardAsync([FromBody] CreateCardRequest request)
        {
            if (request == null) return BadRequest("Invalid card data.");

            var createdCard = await _cardService.CreateCardAsync(request);

            return CreatedAtAction(nameof(CreateCardAsync), new { id = createdCard.Id }, createdCard);
        }

        [HttpPost("purchase")]
        public async Task<IActionResult> MakePurchaseAsync([FromBody] PurchaseRequest request)
        {
            try
            {
                await _cardService.MakeCardPurchaseAsync(request);
                return Ok(new { Message = "Purchase successful." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
