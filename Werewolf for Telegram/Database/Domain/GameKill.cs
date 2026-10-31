using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Table("GameKill")]
public partial class GameKill
{
    [Key]
    public int Id { get; set; }

    public int GameId { get; set; }

    public int KillerId { get; set; }

    public int VictimId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime TimeStamp { get; set; }

    public int KillMethodId { get; set; }

    public int Day { get; set; }

    [ForeignKey("GameId")]
    [InverseProperty("GameKills")]
    public virtual Game Game { get; set; }

    [ForeignKey("KillMethodId")]
    [InverseProperty("GameKills")]
    public virtual KillMethod KillMethod { get; set; }

    [ForeignKey("KillerId")]
    [InverseProperty("GameKills")]
    public virtual Player Killer { get; set; }
}

