using Microsoft.EntityFrameworkCore;
using NearU_Backend_Revised.Data;
using NearU_Backend_Revised.Models;
using NearU_Backend_Revised.Repositories.Interfaces;

namespace NearU_Backend_Revised.Repositories
{
    pubilc class MenuItemRepository : IMenuItemRepository
    {
        public readonly ApplicationDbContext _context;
        
        public MenuItemRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuItem>> GetShopById(string shopId)
        {
            return await _context.Menuitems
                .Where(mi => mi.FoodShop.Id == shopId)
                .OrderBy(mi => mi.Name)
                .ToListAsync();
        }

        public async Task<MenuItem?> GetByIdAsync(string id)
        {
            return await _context.MenuItems
                .FindAsync(id);
        }

        public async Task<MenuItem> CreateAsync(MenuItem item)
        {
            _context.MenuItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<MenuItem?> UpdateAsync(MenuItem item)
        {
            _context.MenuItems.Update(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            if (item == null) return false;

            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}