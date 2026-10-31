using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Keyless]
public partial class v_SummaryTotal
{
    public int? Players { get; set; }

    public int? Games { get; set; }

    public int? Groups { get; set; }

    public int? Deaths { get; set; }

    public int? Survivors { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? Since { get; set; }
}

