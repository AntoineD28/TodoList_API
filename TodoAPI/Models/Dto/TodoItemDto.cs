using System.Text.Json.Serialization;

namespace TodoAPI.Models.Dto
{
    public class TodoItemPostDto
    {
        public required string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int UserId { get; set; }
    }

    public class TodoItemUpdateDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = null!;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("isCompleted")]
        public bool IsCompleted { get; set; }
    }
}
