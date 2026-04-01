using System.ComponentModel.DataAnnotations;

namespace NearU_Backend_Revised.DTOs.Job
{
    public class CreateJob
    {
        [Required(ErrorMessage = "Job title is required")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Company name is required")]
        public string Company { get; set; } = null!;

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; } = null!;

        [Required(ErrorMessage = "Pay information is required")]
        public string PayRange { get; set; } = null!;

        [Required(ErrorMessage = "Job type is required")]
        public string JobType { get; set; } = null!;

        [Required(ErrorMessage = "Job category is required")]
        public string Category { get; set; } = null!;

        public string? Logo { get; set; }

        [Required(ErrorMessage = "Job description is required")]
        public string Description { get; set; } = null!;

        public string? LongDescription { get; set; }

        public List<string>? Requirements { get; set; }

        public List<string>? Tags { get; set; }
        public string? Requirements { get; set; }

        public string? Tags { get; set; }

        public bool IsNew { get; set; } = true;
    }
}