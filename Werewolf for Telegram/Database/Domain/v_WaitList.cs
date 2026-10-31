using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Keyless]
public partial class v_WaitList
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Expr1 { get; set; }
}

