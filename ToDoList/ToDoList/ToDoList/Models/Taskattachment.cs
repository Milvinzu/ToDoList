using System;
using System.Collections.Generic;

namespace ToDoList.Models;

public partial class Taskattachment
{
    public int Id { get; set; }

    public int? TaskId { get; set; }

    public string? FileName { get; set; }

    public string? FileUrl { get; set; }

    public virtual Task? Task { get; set; }
}
