namespace NearU_Backend_Revised.DTOs.FoodShop
{
    public class FoodShopResponse
    {
        public int Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}