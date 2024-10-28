﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TodoAPI.Models;

public partial class TodoItem
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? UpdateTime { get; set; }

    public DateTime CreateTime { get; set; }

    public int UserId { get; set; }

    // To avoid cycle references
    [JsonIgnore]
    public User User { get; set; } = null!;
}
