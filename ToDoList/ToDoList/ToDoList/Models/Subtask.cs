using System;
using System.Collections.Generic;

namespace ToDoList.Models;

public partial class Subtask
{
    public int Id { get; set; }

    public int? TaskId { get; set; }

    public string Title { get; set; } = null!;

    public bool? Completed { get; set; }

    public virtual Task? Task { get; set; }
}
