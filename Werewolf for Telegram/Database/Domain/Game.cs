using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Table("Game")]
public partial class Game
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string GroupName { get; set; }

    public long GroupId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TimeStarted { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? TimeEnded { get; set; }

    [StringLength(50)]
    public string Winner { get; set; }

    public int? GrpId { get; set; }

    [StringLength(50)]
    public string Mode { get; set; }

    public bool? Beta { get; set; }

    [InverseProperty("Game")]
    public virtual ICollection<GameKill> GameKills { get; set; } = new List<GameKill>();

    [InverseProperty("Game")]
    public virtual ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();

    [ForeignKey("GrpId")]
    [InverseProperty("Games")]
    public virtual Group Grp { get; set; }
}

