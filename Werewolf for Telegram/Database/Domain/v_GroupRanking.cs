using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Keyless]
public partial class v_GroupRanking
{
    public int GroupId { get; set; }

    public long TelegramId { get; set; }

    [Required]
    [StringLength(450)]
    public string Language { get; set; }

    public string Description { get; set; }

    public string GroupLink { get; set; }

    [Column(TypeName = "decimal(18, 10)")]
    public decimal? Ranking { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime LastRefresh { get; set; }

    [Required]
    public string Name { get; set; }
}

