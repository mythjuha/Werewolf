using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Keyless]
public partial class v_WinRatio
{
    public int? Players { get; set; }

    public int? Wins { get; set; }

    [StringLength(50)]
    public string Team { get; set; }

    [Column("%", TypeName = "numeric(26, 12)")]
    public decimal? _ { get; set; }
}

