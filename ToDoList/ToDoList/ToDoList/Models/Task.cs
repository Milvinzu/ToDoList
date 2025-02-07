using System;
using System.Collections.Generic;

namespace ToDoList.Models;

public partial class Task
{
    public int Id { get; set; }

    public int? ListId { get; set; }

    public int? UserId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime? DueDate { get; set; }

    public int? Priority { get; set; }

    public bool? Completed { get; set; }
    public DateTime? ReminderTime { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual List? List { get; set; }

    public virtual ICollection<Recurringtask> Recurringtasks { get; set; } = new List<Recurringtask>();

    public virtual ICollection<Subtask> Subtasks { get; set; } = new List<Subtask>();

    public virtual ICollection<Taskattachment> Taskattachments { get; set; } = new List<Taskattachment>();

    public virtual ICollection<Tasktag> Tasktags { get; set; } = new List<Tasktag>();

    public virtual User? User { get; set; }
}
