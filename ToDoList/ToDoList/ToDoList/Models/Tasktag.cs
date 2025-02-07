using System;
using System.Collections.Generic;

namespace ToDoList.Models;

public partial class Tasktag
{
    public int Id { get; set; }
    public int TaskId { get; set; }

    public string Tag { get; set; } = null!;

    public virtual Task Task { get; set; } = null!;
}
