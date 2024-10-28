using System;
using System.Collections.Generic;

namespace TodoAPI.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime CreateTime { get; set; }

    public ICollection<TodoItem>? TodoItems { get; set; } = null!;

    public User(string username, string email, string password)
    {
        Username=username;
        Email=email;
        Password=password;
    }
}
