using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Keyless]
public partial class v_PublicGroup
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public string Language { get; set; }

    public int? MemberCount { get; set; }

    public string GroupLink { get; set; }
}

