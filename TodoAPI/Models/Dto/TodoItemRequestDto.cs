namespace TodoAPI.Models.Dto
{
    public class TodoItemRequestDto
    {
        public required string Title { get; set; } = null!;

        public string? Description { get; set; }

        public int UserId { get; set; }
    }
}
