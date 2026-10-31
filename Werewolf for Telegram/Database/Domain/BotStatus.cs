using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Table("BotStatus", Schema = "db_owner")]
public partial class BotStatu
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    [Unicode(false)]
    public string BotName { get; set; }

    [Required]
    [Column("BotStatus")]
    [StringLength(50)]
    [Unicode(false)]
    public string BotStatus { get; set; }

    [Required]
    [Unicode(false)]
    public string BotLink { get; set; }
}

