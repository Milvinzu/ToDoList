using System;
using System.Collections.Generic;

namespace ToDoList.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<List> Lists { get; set; } = new List<List>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Taskcollaborator> Taskcollaborators { get; set; } = new List<Taskcollaborator>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();
}
