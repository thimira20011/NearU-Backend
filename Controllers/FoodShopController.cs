using Microsoft.AspNetCore.Mvc;
using NearU_Backend_Revised.DTOs.FoodShop;
using NearU_Backend_Revised.Services.Interfaces;

namespace NearU_Backend_Revised.Controllers
{
    [ApiController] //validate incoming req and return 400 if bad
    [Route("api/foodshops")] //base route

    public class FoodShopController : ControllerBase
    {
        private readonly IFoodShopService _service;

        public FoodShopController(IFoodShopService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var shops = await _service.GetAllShopsAsync();
            return Ok(shops);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var shop = await _service.GetShopByIdAsync(id);
            if (shop == null)
                return NotFound(new { message = "Shop not found" });
            return Ok(shop);
        }

        [HttpPost]
        [Consumes("multipart/form-data")] //accept form data for image upload not json
        public async Task<IActionResult> Create([FromForm] CreateFoodShop request)
        {
            var shop = await _service.CreateShopAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = shop.Id }, shop);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(string id, [FromForm] UpdateFoodShop request)
        {
            var shop = await _service.UpdateShopAsync(id, request);
            if (shop == null)
                return NotFound(new { message = "Shop not found" });
            return Ok(shop);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _service.DeleteShopAsync(id);
            if (!deleted)
                return NotFound(new { message = "Shop not found" });
            return NoContent(); //204 successful delete
        }
    }
        
}