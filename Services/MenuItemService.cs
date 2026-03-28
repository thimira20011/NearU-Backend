using NearU_Backend_Revised.DTOs.MenuItem;
using NearU_Backend_Revised.Models;
using NearU_Backend_Revised.Repositories.Interfaces;
using NearU_Backend_Revised.Services.Interfaces;

namespace NearU_Backend_Revised.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _itemrepository;
        private readonly IFoodShopRepository _shoprepository;

        public MenuItemService(IMenuItemRepository itemrepository, IFoodShopRepository shoprepository)
        {
            _itemrepository = itemrepository;
            _shoprepository = shoprepository;
        }

        public async Task<IEnumerable<MenuItemResponse>> GetItemsByShopAsync(string shopId)
        {
            var items = await _itemrepository.GetByShopIdAsync(shopId);
            return items.Select(item => MapToResponse(item));
        }

        public async Task<MenuItemResponse?> GetItemByIdAsync(string id)
        {
            var item = await _itemrepository.GetByIdAsync(id);
            if (item == null) return null;
            retutn MaptoResponse(item);
        }

        public async Task<MenuItemResponse?> CreateItemAsync(stirng shopId, CreateMenuItem dto)
        {
            var shopExists = await _shoprepository.GetByIdAsync(shopId);
            if(!shopExists) return null;

            var item = new MenuItem
            {
                Id = Guid.NewGuid().ToString(),
                FoodShopId = shopId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Photo = dto.Photo,
                CreatedAt = DateTime.UtcNow,
            };

            var created = await _itemrepository.CreateAsync(item);
            return MapToResponse(created);
        }

        public async Task<MenuItemResponse?> UpdateItemAsync(string id, UpdateMenuItem dto)
        {
            var item = await _itemrepository.GetByIdAsync(id);
            if (item == null) return null;

            item.Name = dto.Name ?? item.Name;
            item.Description = dto.Description ?? item.Description;
            item.PhotoUrl = dto.PhotoUrl ?? item.PhotoUrl;
            item.Price = dto.Price ?? item.Price;

            var updated = await _itemrepository.UpdateAsync(item);
            if (updated == null) retun null;
            return MapToResponse(updated);
        }

        public async void DeleteItemAsync(string id) 
        {
            return await _itemrepository.DeleteAsync(id);
        }

        private MenuItemResponse MapToResponse(MenuItem item)
        {
            return new MenuItemResponse
            {
                Id = item.Id,
                FoodShopId = item.FoodShopId,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                PhotoUrl = item.PhotoUrl,
                CreatedAt = item.CreatedAt,
            };
        }
}