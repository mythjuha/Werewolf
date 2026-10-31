using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Table("GroupRanking")]
public partial class GroupRanking
{
    public int GroupId { get; set; }

    [Required]
    [StringLength(450)]
    public string Language { get; set; }

    public int PlayersCount { get; set; }

    [Column(TypeName = "decimal(18, 10)")]
    public decimal MinutesPlayed { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime LastRefresh { get; set; }

    [Column(TypeName = "decimal(18, 10)")]
    public decimal? Ranking { get; set; }

    [Key]
    public int Id { get; set; }

    public int GamesPlayed { get; set; }

    public bool? Show { get; set; }
}

