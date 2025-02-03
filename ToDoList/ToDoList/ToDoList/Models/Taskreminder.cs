using System;
using System.Collections.Generic;

namespace ToDoList.Models;

public partial class Taskreminder
{
    public int Id { get; set; }

    public int? TaskId { get; set; }

    public DateTime? ReminderTime { get; set; }

    public virtual Task? Task { get; set; }
}
