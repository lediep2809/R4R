using System;
using System.Collections.Generic;

namespace R4R_API.Models;

public partial class imgRoom
{
    public string Id { get; set; } = null!;

    public string? imgbase64 { get; set; }

    public string? idroom { get; set; }
}
