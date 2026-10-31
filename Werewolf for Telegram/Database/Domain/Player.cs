using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Database;

[Table("Player")]
public partial class Player
{
    [Key]
    public int Id { get; set; }

    public long TelegramId { get; set; }

    [Required]
    public string Name { get; set; }

    public string UserName { get; set; }

    public bool? Banned { get; set; }

    public string BannedBy { get; set; }

    public bool? HasPM { get; set; }

    public string BanReason { get; set; }

    public string ImageFile { get; set; }

    public string Language { get; set; }

    public int? TempBanCount { get; set; }

    public bool? HasPM2 { get; set; }

    public bool? HasDebugPM { get; set; }

    public long? Achievements { get; set; }

    [StringLength(128)]
    public string WebUserId { get; set; }

    public int? DonationLevel { get; set; }

    public bool? Founder { get; set; }

    public string CustomGifSet { get; set; }

    public bool? GifPurchased { get; set; }

    [MaxLength(50)]
    public byte[] NewAchievements { get; set; }

    [InverseProperty("Killer")]
    public virtual ICollection<GameKill> GameKills { get; set; } = new List<GameKill>();

    [InverseProperty("Player")]
    public virtual ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();
}

