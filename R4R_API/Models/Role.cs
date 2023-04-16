using System;
using System.Collections.Generic;

namespace R4R_API.Models;

public partial class Role
{
    public string Id { get; set; } = null!;

    public string? Code { get; set; }

    public string? Name { get; set; }
}
