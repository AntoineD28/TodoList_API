using System;
using System.Collections.Generic;

namespace TodoAPI.Models;

public partial class TodoItem
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime CreateTime { get; set; }
}
