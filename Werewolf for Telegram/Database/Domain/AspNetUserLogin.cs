using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[PrimaryKey("LoginProvider", "ProviderKey", "UserId")]
public partial class AspNetUserLogin
{
    [Key]
    [StringLength(128)]
    public string LoginProvider { get; set; }

    [Key]
    [StringLength(128)]
    public string ProviderKey { get; set; }

    [Key]
    [StringLength(128)]
    public string UserId { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("AspNetUserLogins")]
    public virtual AspNetUser User { get; set; }
}

