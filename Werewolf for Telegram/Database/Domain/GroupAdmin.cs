using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Keyless]
[Table("GroupAdmin")]
public partial class GroupAdmin
{
    public int GroupId { get; set; }

    public int PlayerId { get; set; }

    [ForeignKey("GroupId")]
    public virtual Group Group { get; set; }

    [ForeignKey("PlayerId")]
    public virtual Player Player { get; set; }
}

