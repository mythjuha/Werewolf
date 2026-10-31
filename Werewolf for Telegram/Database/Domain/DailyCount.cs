using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Table("DailyCount")]
public partial class DailyCount
{
    [Key]
    public int Id { get; set; }

    public DateOnly Day { get; set; }

    public int Groups { get; set; }

    public int Games { get; set; }

    public int Users { get; set; }
}

