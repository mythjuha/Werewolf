using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Table("ContestTerms", Schema = "db_owner")]
public partial class ContestTerm
{
    [Key]
    public int TelegramId { get; set; }

    public bool AgreedTerms { get; set; }
}

