using System;
using System.Collections.Generic;

namespace ToDoList.Models;

public partial class Taskcollaborator
{
    public int ListId { get; set; }

    public int UserId { get; set; }

    public string? PermissionLevel { get; set; }

    public virtual List List { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
