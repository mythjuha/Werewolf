using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Table("KillMethod")]
public partial class KillMethod
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [InverseProperty("KillMethod")]
    public virtual ICollection<GameKill> GameKills { get; set; } = new List<GameKill>();
}

