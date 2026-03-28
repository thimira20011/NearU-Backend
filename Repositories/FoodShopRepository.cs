using Microsoft.EntityFrameworkCore;
using NearU_Backend_Revised.Data;
using NearU_Backend_Revised.Models;
using NearU_Backend_Revised.Repositories.Interfaces;

namespace NearU_Backend_Revised.Repositories
{
    public class FoodShopRepository : IFoodShopRepository
    {
        private readonly ApplicationDbContext _context;

        public FoodShopRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FoodShop>> GetAllAsync()
        {
            return await _context.FoodShop
                .OrderByDescending(fs => fs.CreatedAt) //sort result newest first
                .ToListAsync(); //get result as a list
        }

        public async Task<FoodShop?> GetByIdAsync(string id)
        {
            return await _context.FoodShop
                .Include(fs => fs.MenuItems)  //also fetch menuitems when fetching shops
                .FirstOrDefaultAsync(fs => fs.Id == id);
        }

        public async Task<FoodShop> CreateAsync(FoodShop shop)
        {
            _context.FoodShop.Add(shop);
            await _context.SaveChangesAsync();
            return shop;
        }

        public async Task<FoodShop?> UpdateAsync(FoodShop shop)
        {
            _context.FoodShop.Update(shop);
            await _context.SaveChangesAsync();
            return shop;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var shop = await _context.FoodShop.FindAsync(id); //faster than firstordefault for primary key
            if (shop == null) return false;

            _context.FoodShop.Remove(shop);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(string id)
        {
            return await _context.FoodShop
                .AnyAsync(fs => fs.Id == id); //return true if any shop exists with the given id
        }
    }
}
