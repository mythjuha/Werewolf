using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Table("Group")]
public partial class Group
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    public long GroupId { get; set; }

    public bool? Preferred { get; set; }

    public string Language { get; set; }

    public bool? DisableNotification { get; set; }

    public string UserName { get; set; }

    public bool? BotInGroup { get; set; }

    public bool? ShowRoles { get; set; }

    [StringLength(50)]
    public string Mode { get; set; }

    public int? DayTime { get; set; }

    public int? NightTime { get; set; }

    public int? LynchTime { get; set; }

    public bool? AllowTanner { get; set; }

    public bool? AllowFool { get; set; }

    public bool? AllowCult { get; set; }

    [StringLength(50)]
    public string ShowRolesEnd { get; set; }

    public int? MaxPlayers { get; set; }

    public bool? DisableFlee { get; set; }

    public string CreatedBy { get; set; }

    public string ImageFile { get; set; }

    public string Description { get; set; }

    public string GroupLink { get; set; }

    public int? MemberCount { get; set; }

    public bool? AllowExtend { get; set; }

    public int? MaxExtend { get; set; }

    public bool? EnableSecretLynch { get; set; }

    public bool? RandomModes { get; set; }

    public long? Flags { get; set; }

    public bool? BetaGroup { get; set; }

    public long? RoleFlags { get; set; }

    [InverseProperty("Grp")]
    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}

