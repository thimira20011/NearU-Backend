using Microsoft.AspNetCore.Mvc;
using NearU_Backend_Revised.DTOs.MenuItem;
using NearU_Backend_Revised.Services.Interfaces;

namespace NearU_Backend_Revised.Controllers
{
    [ApiController]
    [Route("api/foodshops/{shopId}/menuItems")]

    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemService _service;

        public MenuItemController(IMenuItemService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string shopId)
        {
            var items = await _service.GetItemsByShopAsync(shopId);
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string shopId, string id)
        {
            var item = await _service.GetItemByIdAsync(id);
            if (item == null)
                return NotFound(new { message = "Item not found" });
            return Ok(item);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create(string shopId, [FromForm] CreateMenuItem request)
        {
            var item = await _service.CreateItemAsync(shopId, request);
            if (item == null)
                return NotFound(new { message = "Shop not found" });
            return CreatedAtAction(nameof(GetById), new { shopId = shopId, id = item.Id }, item);
        }

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Update(string shopId, string id, [FromForm] UpdateMenuItem request)
        {
            var item = await _service.UpdateItemAsync(id, request);
            if (item == null)
                return NotFound(new { message = "Shop not found" });
            return Ok(item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delte(string shopId, string id)
        {
            var deleted = await _service.DeleteItemAsync(id);
            if (!deleted)
                return NotFound(new { message = "Shop not found" });
            return NoContent();
        }
    }    
}