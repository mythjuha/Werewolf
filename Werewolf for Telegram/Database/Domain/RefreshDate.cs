using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Table("RefreshDate")]
public partial class RefreshDate
{
    [Key]
    [StringLength(1)]
    [Unicode(false)]
    public string Lock { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }
}

