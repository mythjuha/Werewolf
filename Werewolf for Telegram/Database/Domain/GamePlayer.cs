using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Table("GamePlayer")]
public partial class GamePlayer
{
    [Key]
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public int GameId { get; set; }

    public bool Survived { get; set; }

    public bool Won { get; set; }

    [StringLength(50)]
    public string Role { get; set; }

    [ForeignKey("GameId")]
    [InverseProperty("GamePlayers")]
    public virtual Game Game { get; set; }

    [ForeignKey("PlayerId")]
    [InverseProperty("GamePlayers")]
    public virtual Player Player { get; set; }
}

