using System;
using System.Collections.Generic;

namespace ToDoList.Models;

public partial class Recurringtask
{
    public int Id { get; set; }

    public int? TaskId { get; set; }

    public string? RecurrenceType { get; set; }

    public DateTime? NextOccurrence { get; set; }

    public virtual Task? Task { get; set; }
}
