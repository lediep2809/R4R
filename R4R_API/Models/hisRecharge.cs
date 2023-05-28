using System;
using System.Collections.Generic;

namespace R4R_API.Models;

public partial class hisRecharge
{
    public string? Id { get; set; }

    public string? userEmail { get; set; } = null!;

    public int? moneyRecharge { get; set; }

    public string? note { get; set; }

    public DateTime? createDate { get; set; }
}
