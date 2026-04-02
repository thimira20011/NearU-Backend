namespace NearU_Backend_Revised.DTOs.Job
{
    public class UpdateJob
    {
        public string? Title { get; set; }
        public string? Company { get; set; }
        public string? Location { get; set; }
        public string? PayRange { get; set; }
        public string? JobType { get; set; }
        public string? Category { get; set; }
        public string? Logo { get; set; }
        public string? Description { get; set; }
        public string? LongDescription { get; set; }
        public List<string>? Requirements { get; set; }
        public List<string>? Tags { get; set; }
        public bool? IsNew { get; set; }
        public PostedByInfo PostedBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string PostedAt { get; set; } = null!;
    }

    public class PostedByInfo
    {
        public string UserId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Avatar { get; set; }
    }
}