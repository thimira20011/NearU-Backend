using System.ComponentModel.DataAnnotations;

namespace NearU_Backend_Revised.DTOs.MenuItem
{
    public class UpdateMenuItem
    {

        public string? Name { get; set; }
        
        public decimal? Price { get; set; }

        public string? Description { get; set; }

        public string? PhotoUrl { get; set; }
    }
}