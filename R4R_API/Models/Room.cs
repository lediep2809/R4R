using System;
using System.Collections.Generic;

namespace R4R_API.Models;

public partial class Room
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Address { get; set; }

    public string Category { get; set; } = null!;

    public string Area { get; set; } = null!;

    public string Capacity { get; set; } = null!;

    public string? Description { get; set; }

    public string Price { get; set; } = null!;

    public string? Deposit { get; set; }

    public string? Electricprice { get; set; }

    public string? Waterprice { get; set; }

    public string? Otherprice { get; set; }

    public string? Houseowner { get; set; }

    public string? Ownerphone { get; set; }

    public string? Createdby { get; set; }

    public string? Activeby { get; set; }

    public DateTime Createddate { get; set; }

    public DateTime? Activedate { get; set; }

    public int? Status { get; set; }
}
