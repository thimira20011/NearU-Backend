using NearU_Backend_Revised.DTOs.FoodShop;
using NearU_Backend_Revised.Models;
using NearU_Backend_Revised.Repositories.Interfaces;
using NearU_Backend_Revised.Services.Interfaces;

namespace NearU_Backend_Revised.Services
{
    public class FoodShopService : IFoodShopService
    {
        private readonly IFoodShopRepository _repository;

        public FoodShopService(IFoodShopRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<FoodShopResponse>> GetAllShopsAsync()
        {
            var shops = await _repository.GetAllAsync();
            return shops.Select(s => MapToResponse(s)); // Corrected: use the parameter 's'
        }

        public async Task<FoodShopResponse?> GetShopByIdAsync(string id)
        {
            var shop = await _repository.GetByIdAsync(id);
            if (shop == null) return null; // Corrected: removed 'shop' outside the parentheses
            return MapToResponse(shop);
        }

        public async Task<FoodShopResponse> CreateShopAsync(CreateFoodShop dto)
        {
            var shop = new FoodShop
            {
                Id = Guid.NewGuid().ToString(), // Corrected: NewGuid instead of NewGuild
                Name = dto.Name,
                Description = dto.Description,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                // PhotoUrl = dto.PhotoUrl, // Removed: Not in current model
                CreatedAt = DateTime.UtcNow,
            };

            var created = await _repository.CreateAsync(shop); // Corrected: removed dot after await
            return MapToResponse(created);
        }

        public async Task<FoodShopResponse?> UpdateShopAsync(string id, UpdateFoodShop dto)
        {
            var shop = await _repository.GetByIdAsync(id);
            if (shop == null) return null;
             
            shop.Name = dto.Name ?? shop.Name; // Corrected: name to Name
            shop.Description = dto.Description;
            shop.Address = dto.Address;
            shop.PhoneNumber = dto.PhoneNumber;
            // shop.PhotoUrl = dto.PhotoUrl; // Removed: Not in current model

            var updated = await _repository.UpdateAsync(shop);
            if (updated == null) return null;
            return MapToResponse(updated);
        }

        public async Task<bool> DeleteShopAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static FoodShopResponse MapToResponse(FoodShop shop) // Corrected method syntax
        {
            return new FoodShopResponse
            {
                Id = shop.Id,
                Name = shop.Name, // Corrected: Name instead of Id
                Description = shop.Description,
                Address = shop.Address,
                PhoneNumber = shop.PhoneNumber,
                // PhotoUrl = shop.PhotoUrl, // Removed: Not in current model
                CreatedAt = shop.CreatedAt, // Corrected: CreatedAt
            };
        }
    }
}
