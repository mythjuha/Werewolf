using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Table("LangPackGif")]
public partial class LangPackGif
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string LanguageName { get; set; }

    public string GifSet { get; set; }
}

