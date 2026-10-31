using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Table("Language")]
public partial class Language
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(100)]
    public string Base { get; set; }

    [Required]
    [StringLength(100)]
    public string Variant { get; set; }

    [Required]
    [StringLength(200)]
    public string FileName { get; set; }

    public bool IsDefault { get; set; }

    [StringLength(10)]
    public string LangCode { get; set; }
}

