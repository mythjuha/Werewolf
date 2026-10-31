using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Keyless]
public partial class v_NonDefaultGroup
{
    [Required]
    public string Name { get; set; }

    public string Language { get; set; }

    public bool? ShowRoles { get; set; }

    [StringLength(50)]
    public string ShowRolesEnd { get; set; }

    [StringLength(50)]
    public string Mode { get; set; }

    public int? DayTime { get; set; }

    public int? NightTime { get; set; }

    public int? LynchTime { get; set; }

    public bool? AllowFool { get; set; }

    public bool? AllowTanner { get; set; }

    public bool? AllowCult { get; set; }

    public string UserName { get; set; }

    public bool? BotInGroup { get; set; }

    public bool? DisableNotification { get; set; }

    public int? MaxPlayers { get; set; }

    public bool? DisableFlee { get; set; }

    public bool? Preferred { get; set; }

    public long GroupId { get; set; }
}

