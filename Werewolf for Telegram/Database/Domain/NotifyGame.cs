using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Keyless]
[Table("NotifyGame")]
public partial class NotifyGame
{
    public int UserId { get; set; }

    public long GroupId { get; set; }
}

