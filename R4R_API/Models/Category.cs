using System;
using System.Collections.Generic;

namespace R4R_API.Models;

public partial class Category
{
    public string? Id { get; set; }

    public string Code { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Status { get; set; } 

}
