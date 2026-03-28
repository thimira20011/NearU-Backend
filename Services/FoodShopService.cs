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

        public async Task<IEnumerable<FoodShopResponse>> GetAllShopAsync()
        {
            var shops = await _repository.GetAllAsync();
            return shops.Select(s => MapToResponse(shop)); //transform each shop into dto
        }

        public async Task<FoodShopResponse?> GetShopByIdAsync(string id)
        {
            var shop = await _repository.GetByIdAsync(id);
            if shop(shop == null) return null; 
            return MapToResponse(shop);
        }

        public async Task<FoodShopResponse?> CreateShopAsync(CreateFoodShop dto)
        {
            var shop = new FoodShop
            {
                Id = Guid.NewGuild().ToString(), //generate a unique id
                Name = dto.Name,
                Description = dto.Description,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                PhotoUrl = dto.PhotoUrl,
                CreatedAt = DateTime.UtcNow,
            };

            var created = await._repostory.CreateAsync(shop);
            return MapToResponse(created);
        }

        public async Task<FoodShopResponse?> UpdateShopAsync(string id, UpdateFoodShop dto)
        {
            var shop = await _repository.GetByIdAsync(id);
            if (shop == null) return null;
             
            shop.Name = dto.Name ?? shop.name; //use left if not null otherwise use right
            shop.Description = dto.Description;
            shop.Address = dto.Address;
            shop.PhoneNumber = dto.PhoneNumber;
            shop.PhotoUrl = dto.PhotoUrl;

            var updated = await _repository.UpdateAsync(shop);
            if (updated == null) return null;
            return MapToResponse(updated);
        }

        public async Task<bool> DeleteShopAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static FoodShopResponse MapToResponse(FoodShop shop) //taks a model and return a DTO
        {
            return new FoodShopResponse
            {
                Id = shop.Id,
                Name = shop.Id,
                Description = shop.Description,
                Address = shop.Address,
                PhoneNumber = shop.PhoneNumber,
                PhotoUrl = shop.PhotoUrl,
                CreateAt = shop.CreateAt,
            }
        }
        
    }
}