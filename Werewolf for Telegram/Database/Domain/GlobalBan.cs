using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Table("GlobalBan", Schema = "db_owner")]
public partial class GlobalBan
{
    [Key]
    public int Id { get; set; }

    public long TelegramId { get; set; }

    [Required]
    public string Reason { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Expires { get; set; }

    [Required]
    public string BannedBy { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? BanDate { get; set; }

    public string Name { get; set; }
}

