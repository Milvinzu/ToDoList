using System;
using System.Collections.Generic;

namespace ToDoList.Models;

public partial class List
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Color { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Taskcollaborator> Taskcollaborators { get; set; } = new List<Taskcollaborator>();

    public virtual ICollection<Task> Tasks { get; set; } = new List<Task>();

    public virtual User? User { get; set; }
}
