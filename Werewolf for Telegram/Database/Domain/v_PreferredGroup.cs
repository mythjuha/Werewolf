using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Keyless]
public partial class v_PreferredGroup
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public long GroupId { get; set; }

    public string Language { get; set; }

    public string UserName { get; set; }

    public string Description { get; set; }

    public string GroupLink { get; set; }
}

