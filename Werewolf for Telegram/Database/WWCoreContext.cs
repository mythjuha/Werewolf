using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Common;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Database;

public partial class WWCoreContext : DbContext
{
    private static string _defaultConnectionString;

    /// <summary>
    /// Configures the connection string used by parameterless contexts created by legacy consumers.
    /// </summary>
    public static void Configure(string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);
        _defaultConnectionString = connectionString;
    }

    public WWCoreContext()
        : this(CreateDefaultOptions())
    {
    }

    public WWCoreContext(DbContextOptions<WWCoreContext> options)
        : base(options)
    {
    }

    private static DbContextOptions<WWCoreContext> CreateDefaultOptions()
    {
        if (string.IsNullOrWhiteSpace(_defaultConnectionString))
        {
            throw new InvalidOperationException("WWCoreContext has not been configured. Call WWCoreContext.Configure with the host configuration connection string before creating a context.");
        }

        return new DbContextOptionsBuilder<WWCoreContext>()
            .UseSqlServer(_defaultConnectionString)
            .Options;
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<BotStatu> BotStatuses { get; set; }

    public virtual DbSet<ContestTerm> ContestTerms { get; set; }

    public virtual DbSet<DailyCount> DailyCounts { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameKill> GameKills { get; set; }

    public virtual DbSet<GamePlayer> GamePlayers { get; set; }

    public virtual DbSet<GlobalBan> GlobalBans { get; set; }

    public virtual DbSet<GlobalStat> GlobalStats { get; set; }

    public virtual DbSet<Group> Groups { get; set; }

    public virtual DbSet<GroupAdmin> GroupAdmins { get; set; }

    public virtual DbSet<GroupRanking> GroupRankings { get; set; }
    public virtual DbSet<GroupRanking> GroupRanking { get; set; }

    public virtual DbSet<GroupStat> GroupStats { get; set; }

    public virtual DbSet<KillMethod> KillMethods { get; set; }

    public virtual DbSet<LangPackGif> LangPackGifs { get; set; }

    public virtual DbSet<Language> Languages { get; set; }
    public virtual DbSet<Language> Language { get; set; }

    public virtual DbSet<NotifyGame> NotifyGames { get; set; }

    public virtual DbSet<Player> Players { get; set; }

    public virtual DbSet<PlayerStat> PlayerStats { get; set; }

    public virtual DbSet<RefreshDate> RefreshDates { get; set; }
    public virtual DbSet<RefreshDate> RefreshDate { get; set; }

    public virtual DbSet<__MigrationHistory> __MigrationHistories { get; set; }

    public virtual DbSet<v_BotInGroup> v_BotInGroups { get; set; }

    public virtual DbSet<v_GroupRanking> v_GroupRankings { get; set; }
    public virtual DbSet<v_GroupRanking> v_GroupRanking { get; set; }

    public virtual DbSet<v_IdleKill24HoursMain> v_IdleKill24HoursMains { get; set; }

    public virtual DbSet<v_InactivePlayersMain> v_InactivePlayersMains { get; set; }

    public virtual DbSet<v_LanguageCount> v_LanguageCounts { get; set; }

    public virtual DbSet<v_NonDefaultGroup> v_NonDefaultGroups { get; set; }

    public virtual DbSet<v_PreferredGroup> v_PreferredGroups { get; set; }

    public virtual DbSet<v_PublicGroup> v_PublicGroups { get; set; }

    public virtual DbSet<v_SummaryTotal> v_SummaryTotals { get; set; }

    public virtual DbSet<v_WaitList> v_WaitLists { get; set; }
    public virtual DbSet<v_WaitList> v_WaitList { get; set; }

    public virtual DbSet<v_WinRatio> v_WinRatios { get; set; }
    public virtual DbSet<getDailyCounts_Result> DailyCountsResult { get; set; }
    public virtual DbSet<GlobalDay1Death_Result1> GlobalDay1DeathResult { get; set; }
    public virtual DbSet<GlobalDay1Lynch_Result1> GlobalDay1LynchResult { get; set; }
    public virtual DbSet<GlobalNight1Death_Result1> GlobalNight1DeathResult { get; set; }
    public virtual DbSet<GlobalSurvivor_Result1> GlobalSurvivorResult { get; set; }
    public virtual DbSet<GroupDay1Death_Result1> GroupDay1DeathResult { get; set; }
    public virtual DbSet<GroupDay1Lynch_Result1> GroupDay1LynchResult { get; set; }
    public virtual DbSet<GroupNight1Death_Result1> GroupNight1DeathResult { get; set; }
    public virtual DbSet<GroupSurvivor_Result1> GroupSurvivorResult { get; set; }
    public virtual DbSet<getPlayTime_Result> GetPlayTimeResult { get; set; }
    public virtual DbSet<getRoles_Result> GetRolesResult { get; set; }
    public virtual DbSet<PlayerMostKilled_Result> PlayerMostKilledResult { get; set; }
    public virtual DbSet<PlayerMostKilledBy_Result> PlayerMostKilledByResult { get; set; }
    public virtual DbSet<PlayerRoles_Result> PlayerRolesResult { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<getDailyCounts_Result>().HasNoKey();
        modelBuilder.Entity<GlobalDay1Death_Result1>().HasNoKey();
        modelBuilder.Entity<GlobalDay1Lynch_Result1>().HasNoKey();
        modelBuilder.Entity<GlobalNight1Death_Result1>().HasNoKey();
        modelBuilder.Entity<GlobalSurvivor_Result1>().HasNoKey();
        modelBuilder.Entity<GroupDay1Death_Result1>().HasNoKey();
        modelBuilder.Entity<GroupDay1Lynch_Result1>().HasNoKey();
        modelBuilder.Entity<GroupNight1Death_Result1>().HasNoKey();
        modelBuilder.Entity<GroupSurvivor_Result1>().HasNoKey();
        modelBuilder.Entity<getPlayTime_Result>().HasNoKey();
        modelBuilder.Entity<getRoles_Result>().HasNoKey();
        modelBuilder.Entity<PlayerMostKilled_Result>().HasNoKey();
        modelBuilder.Entity<PlayerMostKilledBy_Result>().HasNoKey();
        modelBuilder.Entity<PlayerRoles_Result>().HasNoKey();

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.Property(e => e.UserId).ValueGeneratedNever();
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AspNetRoles");
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AspNetUsers");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK_dbo.AspNetUserRoles");
                        j.ToTable("AspNetUserRoles");
                        j.IndexerProperty<string>("UserId").HasMaxLength(128);
                        j.IndexerProperty<string>("RoleId").HasMaxLength(128);
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.AspNetUserClaims");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasConstraintName("FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId");
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey, e.UserId }).HasName("PK_dbo.AspNetUserLogins");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasConstraintName("FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId");
        });

        modelBuilder.Entity<ContestTerm>(entity =>
        {
            entity.Property(e => e.TelegramId).ValueGeneratedNever();
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasOne(d => d.Grp).WithMany(p => p.Games)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Game_Group");
        });

        modelBuilder.Entity<GameKill>(entity =>
        {
            entity.HasOne(d => d.Game).WithMany(p => p.GameKills).HasConstraintName("FK_GameKill_Game");

            entity.HasOne(d => d.KillMethod).WithMany(p => p.GameKills).HasConstraintName("FK_GameKill_KillMethod");

            entity.HasOne(d => d.Killer).WithMany(p => p.GameKills).HasConstraintName("FK_GameKill_Killer");
        });

        modelBuilder.Entity<GamePlayer>(entity =>
        {
            entity.HasOne(d => d.Game).WithMany(p => p.GamePlayers)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GamePlayer_Game");

            entity.HasOne(d => d.Player).WithMany(p => p.GamePlayers).HasConstraintName("FK_GamePlayer_Player");
        });

        modelBuilder.Entity<GlobalStat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GlobalSt__3214EC071820A9D5");
        });

        modelBuilder.Entity<GroupAdmin>(entity =>
        {
            entity.HasOne(d => d.Group).WithMany().HasConstraintName("FK_GroupAdmin_Group");

            entity.HasOne(d => d.Player).WithMany().HasConstraintName("FK_GroupAdmin_Player");
        });

        modelBuilder.Entity<GroupRanking>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_groupranking");
        });

        modelBuilder.Entity<GroupStat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GroupSta__3214EC07D0FE1CB9");
        });

        modelBuilder.Entity<Player>(entity =>
        {
            entity.Property(e => e.DonationLevel).HasDefaultValue(0);
            entity.Property(e => e.GifPurchased).HasDefaultValue(false);
            entity.Property(e => e.Language).HasDefaultValue("English", "DF_Player_Language");
        });

        modelBuilder.Entity<PlayerStat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__PlayerSt__3214EC07B1D226B3");
        });

        modelBuilder.Entity<RefreshDate>(entity =>
        {
            entity.Property(e => e.Lock)
                .HasDefaultValue("X")
                .IsFixedLength();
        });

        modelBuilder.Entity<__MigrationHistory>(entity =>
        {
            entity.HasKey(e => new { e.MigrationId, e.ContextKey }).HasName("PK_dbo.__MigrationHistory");
        });

        modelBuilder.Entity<v_BotInGroup>(entity =>
        {
            entity.ToView("v_BotInGroups", "db_owner");
        });

        modelBuilder.Entity<v_GroupRanking>(entity =>
        {
            entity.ToView("v_GroupRanking");
        });

        modelBuilder.Entity<v_IdleKill24HoursMain>(entity =>
        {
            entity.ToView("v_IdleKill24HoursMain");
        });

        modelBuilder.Entity<v_InactivePlayersMain>(entity =>
        {
            entity.ToView("v_InactivePlayersMain", "db_owner");
        });

        modelBuilder.Entity<v_LanguageCount>(entity =>
        {
            entity.ToView("v_LanguageCounts", "db_owner");
        });

        modelBuilder.Entity<v_NonDefaultGroup>(entity =>
        {
            entity.ToView("v_NonDefaultGroups", "db_owner");
        });

        modelBuilder.Entity<v_PreferredGroup>(entity =>
        {
            entity.ToView("v_PreferredGroups", "db_owner");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<v_PublicGroup>(entity =>
        {
            entity.ToView("v_PublicGroups", "db_owner");

            entity.Property(e => e.Id).ValueGeneratedOnAdd();
        });

        modelBuilder.Entity<v_SummaryTotal>(entity =>
        {
            entity.ToView("v_SummaryTotals", "db_owner");
        });

        modelBuilder.Entity<v_WaitList>(entity =>
        {
            entity.ToView("v_WaitList", "db_owner");
        });

        modelBuilder.Entity<v_WinRatio>(entity =>
        {
            entity.ToView("v_WinRatios", "db_owner");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    public IQueryable<getDailyCounts_Result> getDailyCounts() =>
        Set<getDailyCounts_Result>().FromSqlRaw("EXEC dbo.getDailyCounts");

    public IQueryable<GlobalDay1Death_Result1> GlobalDay1Death() =>
        Set<GlobalDay1Death_Result1>().FromSqlRaw("EXEC dbo.GlobalDay1Death");

    public IQueryable<GlobalDay1Lynch_Result1> GlobalDay1Lynch() =>
        Set<GlobalDay1Lynch_Result1>().FromSqlRaw("EXEC dbo.GlobalDay1Lynch");

    public IQueryable<GlobalNight1Death_Result1> GlobalNight1Death() =>
        Set<GlobalNight1Death_Result1>().FromSqlRaw("EXEC dbo.GlobalNight1Death");

    public IQueryable<GlobalSurvivor_Result1> GlobalSurvivor() =>
        Set<GlobalSurvivor_Result1>().FromSqlRaw("EXEC dbo.GlobalSurvivor");

    public IQueryable<GroupDay1Death_Result1> GroupDay1Death(long? groupid) =>
        Set<GroupDay1Death_Result1>().FromSqlRaw("EXEC dbo.GroupDay1Death @groupid={0}", groupid);

    public IQueryable<GroupDay1Lynch_Result1> GroupDay1Lynch(long? groupid) =>
        Set<GroupDay1Lynch_Result1>().FromSqlRaw("EXEC dbo.GroupDay1Lynch @groupid={0}", groupid);

    public IQueryable<GroupNight1Death_Result1> GroupNight1Death(long? groupid) =>
        Set<GroupNight1Death_Result1>().FromSqlRaw("EXEC dbo.GroupNight1Death @groupid={0}", groupid);

    public IQueryable<GroupSurvivor_Result1> GroupSurvivor(long? groupid) =>
        Set<GroupSurvivor_Result1>().FromSqlRaw("EXEC dbo.GroupSurvivor @groupid={0}", groupid);

    public IQueryable<getPlayTime_Result> getPlayTime(int? playerCount) =>
        Set<getPlayTime_Result>().FromSqlRaw("EXEC dbo.getPlayTime @playerCount={0}", playerCount);

    public IQueryable<getRoles_Result> getRoles(string groupName) =>
        Set<getRoles_Result>().FromSqlRaw("EXEC dbo.getRoles @groupName={0}", groupName);

    public IQueryable<PlayerMostKilled_Result> PlayerMostKilled(long? pid) =>
        Set<PlayerMostKilled_Result>().FromSqlRaw("EXEC dbo.PlayerMostKilled @pid={0}", pid);

    public IQueryable<PlayerMostKilledBy_Result> PlayerMostKilledBy(long? pid) =>
        Set<PlayerMostKilledBy_Result>().FromSqlRaw("EXEC dbo.PlayerMostKilledBy @pid={0}", pid);

    public IQueryable<PlayerRoles_Result> PlayerRoles(long? pid) =>
        Set<PlayerRoles_Result>().FromSqlRaw("EXEC dbo.PlayerRoles @pid={0}", pid);

    public int RestoreAccount(long? oldTGId, long? newTGId) =>
        Database.ExecuteSqlRaw("EXEC dbo.RestoreAccount @oldTGId={0}, @newTGId={1}", oldTGId, newTGId);

    public int ExecuteSql(string sql)
    {
        var connection = Database.GetDbConnection();
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        if (connection.State != System.Data.ConnectionState.Open)
            connection.Open();
        return command.ExecuteNonQuery();
    }

    public List<T> ExecuteQuery<T>(string sql) where T : new()
    {
        var result = new List<T>();
        var connection = Database.GetDbConnection();
        using var command = connection.CreateCommand();
        command.CommandText = sql;
        if (connection.State != System.Data.ConnectionState.Open)
            connection.Open();
        using var reader = command.ExecuteReader();
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .ToDictionary(x => x.Name, StringComparer.OrdinalIgnoreCase);
        while (reader.Read())
        {
            var item = new T();
            for (var i = 0; i < reader.FieldCount; i++)
            {
                if (!properties.TryGetValue(reader.GetName(i), out var property) || reader.IsDBNull(i))
                    continue;
                property.SetValue(item, Convert.ChangeType(reader.GetValue(i), Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType));
            }
            result.Add(item);
        }
        return result;
    }

    public IQueryable<int?> GetIdleKills24Hours(long? userid) =>
        Database.SqlQueryRaw<int?>("EXEC dbo.GetIdleKills24Hours @userid={0}", userid);

    public IQueryable<int?> GetGroupIdleKills24Hours(long? userid, long? groupid) =>
        Database.SqlQueryRaw<int?>("EXEC dbo.GetGroupIdleKills24Hours @userid={0}, @groupid={1}", userid, groupid);

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
