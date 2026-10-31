using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Keyless]
public partial class v_InactivePlayersMain
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public int TelegramId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? last { get; set; }
}

