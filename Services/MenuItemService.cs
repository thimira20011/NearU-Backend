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
            return MapToResponse(item); // Corrected typo
        }

        public async Task<MenuItemResponse?> CreateItemAsync(string shopId, CreateMenuItem dto) // Corrected 'stirng'
        {
            var shopExists = await _shoprepository.GetByIdAsync(shopId);
            if (shopExists == null) return null; // Corrected check

            var item = new MenuItem
            {
                Id = Guid.NewGuid().ToString(),
                FoodShopId = shopId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                PhotoUrl = dto.PhotoUrl, // Corrected property
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
            if (dto.Price.HasValue)
            {
                item.Price = dto.Price.Value;
            }

            var updated = await _itemrepository.UpdateAsync(item);
            if (updated == null) return null; // Corrected typo
            return MapToResponse(updated);
        }

        public async Task<bool> DeleteItemAsync(string id) // Corrected signature
        {
            return await _itemrepository.DeleteAsync(id);
        }

        private static MenuItemResponse MapToResponse(MenuItem item) // Made static for better performance
        {
            return new MenuItemResponse
            {
                Id = item.Id,
                FoodShopId = item.FoodShopId,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                PhotoUrl = item.PhotoUrl,
            };
        }
    }
}
