using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Keyless]
public partial class v_BotInGroup
{
    public int? Groups { get; set; }

    [Column("Has Bot?")]
    [StringLength(3)]
    [Unicode(false)]
    public string Has_Bot_ { get; set; }
}

