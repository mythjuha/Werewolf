using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Keyless]
public partial class v_IdleKill24HoursMain
{
    public int? Idles { get; set; }

    [Required]
    public string Name { get; set; }

    public string UserName { get; set; }
}

