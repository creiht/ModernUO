using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Server.Collections;
using Server.Json;
using Server.Logging;
using Server.Network;
using Server.Targeting;

namespace Server;

public enum MusicName
{
    Invalid = -1,   // File listed in default `Config.txt`
    OldUlt01 = 0,   // turfin,loop
    Create1,        // turfin,loop
    DragFlit,       // turfin,loop
    OldUlt02,       // turfin,loop
    OldUlt03,       // turfin,loop
    OldUlt04,       // turfin,loop
    OldUlt05,       // turfin,loop
    OldUlt06,       // turfin,loop
    Stones2,        // stones1
    Britain1,       // britainpos,loop
    Britain2,       // britain1
    Bucsden,        // bucsden,loop
    Jhelom,         // jhelom
    LBCastle,       // lbc
    Linelle,        // linelle
    Magincia,       // newmagincia,loop
    Minoc,          // minocpos,loop
    Ocllo,          // valoriapos,loop
    Samlethe,       // ambrosia,loop
    Serpents,       // stones
    Skarabra,       // scarabreapos,loop
    Trinsic,        // trinsicpos,loop
    Vesper,         // vesper1
    Wind,           // yew1
    Yew,            // yewpos,loop
    Cave01,         // dungeon
    Dungeon9,       // dragonshi,loop
    Forest_a,       // citynightedit,loop
    InTown01,       // walking,loop
    Jungle_a,       // citynightedit,loop
    Mountn_a,       // walking,loop
    Plains_a,       // citynightedit,loop
    Sailing,        // boattravel
    Swamp_a,        // citynightedit,loop
    Tavern01,       // tavern1
    Tavern02,       // tavern2
    Tavern03,       // tavern3
    Tavern04,       // pubtune,loop
    Combat1,        // goodevil,loop
    Combat2,        // humanoids,loop
    Combat3,        // gargoyles,loop
    Approach,       // turfin,loop
    Death,          // deathtune
    Victory,        // victory
    BTCastle,       // overlordv2
    Nujelm,         // nujelm
    Dungeon2,       // ragonslo,loop
    Cove,           // cove,loop
    Moonglow,       // moonglowpos,loop
    Zento,          // zento,loop
    TokunoDungeon,  // tokunodungeon,loop
    Taiko,          // taiko,loop
    DreadHornArea,  // dread_horn_area,loop
    ElfCity,        // elf_city_1,loop
    GrizzleDungeon, // grizzle_dungeon,loop
    MelisandesLair, // melisandes_lair,loop
    ParoxysmusLair, // paroxysmus_lair,loop
    GwennoConversation, // ConversationWithGwenno.mp3
    GoodEndGame,    // GoodEndGame.mp3
    GoodVsEvil,     // GoodVsEvil.mp3
    GreatEarthSerpents, // GreatEarthSerpentsTheme.mp3
    Humanoids_U9,   // HumanoidsU9.mp3
    MinocNegative,  // MinocNegative.mp3
    Paws,           // Paws.mp3
    SelimsBar,      // SelimsBar.mp3
    SerpentIsleCombat_U7,   // UltimaVIISerpentIsleCombat.mp3
    ValoriaShips,   // ValoriaShips.mp3
    TheWanderer,    // TheWanderer.mp3
    Castle,         // Castle.mp3
    Festival,       // Festival.mp3
    Honor,          // Honor.mp3
    Medieval,       // Medieval.mp3
    BattleOnStones, // BattleOnStones.mp3
    Docktown,       // Docktown.mp3
    GargoyleQueen,  // GargoyleQueen.mp3
    GenericCombat,  // GenericCombat.mp3
    Holycity,       // Holycity.mp3 
    HumanLevel,     // HumanLevel.mp3
    LoginLoop,      // LoginLoop.mp3,loop
    NorthernForestBattleonStones,   // NorthernForestBattleonStones.mp3
    PrimevalLich,   // PrimevalLich.mp3
    QueenPalace,    // QueenPalace.mp3
    RoyalCity,      // RoyalCity.mp3
    SlasherVeil,    // SlasherVeil.mp3
    StygianAbyss,   // StygianAbyss.mp3
    StygianDragon,  // StygianDragon.mp3 
    Void,           // Void.mp3
    CodexShrine,    // CodexShrine.mp3
    AnvilStrikeInMinoc, // AnvilStrikeInMinoc.mp3
    ASkaranLullaby,     // ASkaranLullaby.mp3
    BlackthornsMarch,   // BlackthornsMarch.mp3
    DupresNightInTrinsic,   // DupresNightInTrinsic.mp3
    FayaxionAndTheSix,  // FayaxionAndTheSix.mp3
    FlightOfTheNexus,   // FlightOfTheNexus.mp3
    GalehavenJaunt,     // GalehavenJaunt.mp3
    JhelomToArms,       // JhelomToArms.mp3
    MidnightInYew,      // MidnightInYew.mp3
    MoonglowSonata,     // MoonglowSonata.mp3
    NewMaginciaMarch,   // NewMaginciaMarch.mp3
    NujelmWaltz,        // NujelmWaltz.mp3
    SherrysSong,        // SherrysSong.mp3
    StarlightInBritain, // StarlightInBritain.mp3
    TheVesperMist,      // TheVesperMist.mp3
    NoMusic = 0x1FFF
}

public class Region : IComparable<Region>, IValueLinkListNode<Region>
{
    private static readonly ILogger logger = LogFactory.GetLogger(typeof(Region));

    public const int DefaultPriority = 50;
    public const int MinZ = sbyte.MinValue;
    public const int MaxZ = sbyte.MaxValue + 1;

    public Region(string name, Map map, int priority, params ReadOnlySpan<Rectangle2D> area) : this(
        name,
        map,
        priority,
        ConvertTo3D(area)
    )
    {
    }

    public Region(string name, Map map, params Rectangle3D[] area) : this(name, map, null, area)
    {
    }

    [JsonConstructor] // Don't include parent, since it is special
    public Region(string name, Map map, int priority, params Rectangle3D[] area) : this(name, map, null, area) =>
        Priority = priority;

    public Region(string name, Map map, Region parent, int priority, params Rectangle3D[] area) : this(name, map, parent, area) =>
        Priority = priority;

    public Region(string name, Map map, Region parent, params ReadOnlySpan<Rectangle2D> area) : this(
        name,
        map,
        parent,
        ConvertTo3D(area)
    )
    {
    }

    public Region(string name, Map map, Region parent, params Rectangle3D[] area)
    {
        Name = name;
        Map = map;
        Parent = parent;
        Area = area;
        Dynamic = true;
        Music = DefaultMusic;

        if (Parent == null)
        {
            ChildLevel = 0;
            Priority = DefaultPriority;
        }
        else
        {
            ChildLevel = Parent.ChildLevel + 1;
            Priority = Parent.Priority;
        }
    }

    // Sectors
    [JsonIgnore]
    public Region Next { get; set; }

    [JsonIgnore]
    public Region Previous { get; set; }

    [JsonIgnore]
    public bool OnLinkList { get; set; }

    // Used during deserialization only
    public Expansion MinExpansion { get; set; } = Expansion.None;

    // Used during deserialization only
    public Expansion MaxExpansion { get; set; } = Expansion.EJ;

    public static List<Region> Regions { get; } = new();

    public static TimeSpan StaffLogoutDelay { get; set; } = TimeSpan.Zero;

    public static TimeSpan DefaultLogoutDelay { get; set; } = TimeSpan.FromMinutes(5.0);

    public string Name { get; }

    public Map Map { get; }

    [JsonInclude]
    [JsonConverter(typeof(RegionByNameConverter))]
    public Region Parent { get; private set; }

    public List<Region> Children { get; } = new();

    public Rectangle3D[] Area { get; }

    public Map.Sector[] Sectors { get; private set; }

    public bool Dynamic { get; }

    public int Priority { get; }

    public int ChildLevel { get; internal set; }

    public bool Registered { get; private set; }

    public Point3D GoLocation { get; set; }

    public MusicName Music { get; set; }

    public bool IsDefault => Map.DefaultRegion == this;
    public virtual MusicName DefaultMusic => Parent?.Music ?? MusicName.Invalid;

    public int CompareTo(Region reg)
    {
        if (reg == null)
        {
            return 1;
        }

        // Dynamic regions go first
        if (Dynamic)
        {
            if (!reg.Dynamic)
            {
                return -1;
            }
        }
        else if (reg.Dynamic)
        {
            return 1;
        }

        var regPriority = reg.Priority;
        return Priority != regPriority ? reg.Priority - Priority : reg.ChildLevel - ChildLevel;
    }

    // This is not optimized. Use sparingly
    public static Region Find(string name, Map map, bool insensitive = false)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return null;
        }

        if (insensitive)
        {
            name = name.ToLower();
        }

        for (var i = 0; i < Regions.Count; i++)
        {
            var region = Regions[i];
            if (region.Map != map)
            {
                continue;
            }

            var rName = region.Name;
            if (insensitive)
            {
                rName = rName.ToLower();
            }

            if (rName == name)
            {
                return region;
            }
        }

        return null;
    }

    public static Region Find(Point3D p, Map map)
    {
        if (map == null)
        {
            return Map.Internal.DefaultRegion;
        }

        var sector = map.GetSector(p);
        var list = sector.Regions;

        for (var i = 0; i < list.Count; ++i)
        {
            var region = list[i];

            if (region.Contains(p))
            {
                return region;
            }
        }

        return map.DefaultRegion;
    }

    public static Rectangle3D ConvertTo3D(Rectangle2D rect) =>
        new(new Point3D(rect.Start, MinZ), new Point3D(rect.End, MaxZ));

    public static Rectangle3D[] ConvertTo3D(ReadOnlySpan<Rectangle2D> rects)
    {
        var ret = new Rectangle3D[rects.Length];

        for (var i = 0; i < ret.Length; i++)
        {
            ret[i] = ConvertTo3D(rects[i]);
        }

        return ret;
    }

    public void Register()
    {
        if (Registered)
        {
            return;
        }

        OnRegister();

        Registered = true;

        if (Parent != null)
        {
            Parent.Children.Add(this);
            Parent.OnChildAdded(this);
        }

        Regions.Add(this);

        Map.RegisterRegion(this);

        var sectors = new List<Map.Sector>();

        for (var i = 0; i < Area.Length; i++)
        {
            var rect = Area[i];

            var start = Map.Bound(new Point2D(rect.Start));
            var end = Map.Bound(new Point2D(rect.End));

            var startSector = Map.GetSector(start);
            var endSector = Map.GetSector(end);

            for (var x = startSector.X; x <= endSector.X; x++)
            {
                for (var y = startSector.Y; y <= endSector.Y; y++)
                {
                    var sector = Map.GetRealSector(x, y);

                    // Region Areas are approximate and will overlap
                    // Don't add them multiple times!
                    if (!sectors.Contains(sector))
                    {
                        sector.OnEnter(this, rect);
                        sectors.Add(sector);
                    }
                }
            }
        }

        Sectors = sectors.ToArray();
    }

    public void Unregister()
    {
        if (!Registered)
        {
            return;
        }

        OnUnregister();

        Registered = false;

        if (Children.Count > 0)
        {
            logger.Warning("Unregistering region '{Region}' with children", this);
        }

        if (Parent != null)
        {
            Parent.Children.Remove(this);
            Parent.OnChildRemoved(this);
        }

        Regions.Remove(this);

        Map.UnregisterRegion(this);

        if (Sectors != null)
        {
            for (var i = 0; i < Sectors.Length; i++)
            {
                Sectors[i].OnLeave(this);
            }
        }

        Sectors = null;
    }

    public bool Contains(Point3D p)
    {
        for (var i = 0; i < Area.Length; i++)
        {
            var rect = Area[i];

            if (rect.Contains(p))
            {
                return true;
            }
        }

        return false;
    }

    // TODO: Memoize this
    public bool IsChildOf(Region region)
    {
        if (region == null)
        {
            return false;
        }

        var p = Parent;

        while (p != null)
        {
            if (p == region)
            {
                return true;
            }

            p = p.Parent;
        }

        return false;
    }

    // TODO: Memoize this
    public T GetRegion<T>() where T : Region
    {
        var r = this;

        do
        {
            if (r is T tr)
            {
                return tr;
            }

            r = r.Parent;
        } while (r != null);

        return null;
    }

    // TODO: Memoize this
    public bool IsPartOf<T1, T2>() where T1 : Region where T2 : Region
    {
        var r = this;

        do
        {
            if (r is T1 or T2)
            {
                return true;
            }

            r = r.Parent;
        } while (r != null);

        return false;
    }

    public Region GetRegion(Type regionType)
    {
        if (regionType == null)
        {
            return null;
        }

        var r = this;

        do
        {
            if (regionType.IsInstanceOfType(r))
            {
                return r;
            }

            r = r.Parent;
        } while (r != null);

        return null;
    }

    public Region GetRegion(string regionName, bool caseSensitive = true)
    {
        if (regionName == null)
        {
            return null;
        }

        var r = this;
        var comparisonType = caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

        do
        {
            if (string.Equals(r.Name, regionName, comparisonType))
            {
                return r;
            }

            r = r.Parent;
        } while (r != null);

        return null;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsPartOf<T>() where T : Region => GetRegion<T>() != null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsPartOf(Region region) => this == region || IsChildOf(region);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool IsPartOf(string regionName, bool caseSensitive = false) => GetRegion(regionName, caseSensitive) != null;

    public virtual bool AcceptsSpawnsFrom(Region region) =>
        AllowSpawn() && (region == this || Parent?.AcceptsSpawnsFrom(region) == true);

    public PooledRefList<Mobile> GetPlayersPooled()
    {
        var list = PooledRefList<Mobile>.Create();
        for (var i = 0; i < Sectors?.Length; i++)
        {
            var sector = Sectors[i];

            foreach (var ns in sector.Clients)
            {
                var player = ns.Mobile;
                if (player?.Deleted == false && player.Region.IsPartOf(this))
                {
                    list.Add(ns.Mobile);
                }
            }
        }

        return list;
    }

    public List<Mobile> GetPlayers()
    {
        List<Mobile> list = [];
        for (var i = 0; i < Sectors?.Length; i++)
        {
            var sector = Sectors[i];

            foreach (var ns in sector.Clients)
            {
                var player = ns.Mobile;
                if (player?.Deleted == false && player.Region.IsPartOf(this))
                {
                    list.Add(ns.Mobile);
                }
            }
        }

        return list;
    }

    public int GetPlayerCount()
    {
        var count = 0;

        for (var i = 0; i < Sectors?.Length; i++)
        {
            var sector = Sectors[i];

            foreach (var ns in sector.Clients)
            {
                var player = ns.Mobile;
                if (player?.Deleted == false && player.Region.IsPartOf(this))
                {
                    count++;
                }
            }
        }

        return count;
    }

    public List<Mobile> GetMobiles()
    {
        var list = new List<Mobile>();

        for (var i = 0; i < Sectors?.Length; i++)
        {
            var sector = Sectors[i];

            foreach (var mobile in sector.Mobiles)
            {
                if (mobile.Region.IsPartOf(this))
                {
                    list.Add(mobile);
                }
            }
        }

        return list;
    }

    public PooledRefList<Mobile> GetMobilesPooled()
    {
        var list = PooledRefList<Mobile>.Create();
        for (var i = 0; i < Sectors?.Length; i++)
        {
            var sector = Sectors[i];

            foreach (var mobile in sector.Mobiles)
            {
                if (mobile.Region.IsPartOf(this))
                {
                    list.Add(mobile);
                }
            }
        }

        return list;
    }

    public int GetMobileCount()
    {
        var count = 0;

        for (var i = 0; i < Sectors?.Length; i++)
        {
            var sector = Sectors[i];

            foreach (var mobile in sector.Mobiles)
            {
                if (mobile.Region.IsPartOf(this))
                {
                    count++;
                }
            }
        }

        return count;
    }

    public List<Item> GetItems()
    {
        var list = new List<Item>();

        for (var i = 0; i < Sectors?.Length; i++)
        {
            var sector = Sectors[i];

            foreach (var item in sector.Items)
            {
                if (Find(item.Location, item.Map).IsPartOf(this))
                {
                    list.Add(item);
                }
            }
        }

        return list;
    }

    public PooledRefList<Item> GetItemsPooled()
    {
        var list = PooledRefList<Item>.Create();

        for (var i = 0; i < Sectors?.Length; i++)
        {
            var sector = Sectors[i];

            foreach (var item in sector.Items)
            {
                if (Find(item.Location, item.Map).IsPartOf(this))
                {
                    list.Add(item);
                }
            }
        }

        return list;
    }

    public int GetItemCount()
    {
        var count = 0;

        for (var i = 0; i < Sectors?.Length; i++)
        {
            var sector = Sectors[i];

            foreach (var item in sector.Items)
            {
                if (Find(item.Location, item.Map).IsPartOf(this))
                {
                    count++;
                }
            }
        }

        return count;
    }

    public override string ToString() => Name ?? GetType().Name;

    public virtual void OnRegister()
    {
    }

    public virtual void OnUnregister()
    {
    }

    public virtual void OnChildAdded(Region child)
    {
    }

    public virtual void OnChildRemoved(Region child)
    {
    }

    public virtual bool OnMoveInto(Mobile m, Direction d, Point3D newLocation, Point3D oldLocation) =>
        m.WalkRegion == null || AcceptsSpawnsFrom(m.WalkRegion);

    public virtual void OnEnter(Mobile m)
    {
    }

    public virtual void OnExit(Mobile m)
    {
    }

    public virtual void MakeGuard(Mobile focus)
    {
        Parent?.MakeGuard(focus);
    }

    public virtual Type GetResource(Type type) => Parent?.GetResource(type) ?? type;

    public virtual bool CanUseStuckMenu(Mobile m) => Parent?.CanUseStuckMenu(m) != false;

    public virtual void OnAggressed(Mobile aggressor, Mobile aggressed, bool criminal)
    {
        Parent?.OnAggressed(aggressor, aggressed, criminal);
    }

    public virtual void OnDidHarmful(Mobile harmer, Mobile harmed)
    {
        Parent?.OnDidHarmful(harmer, harmed);
    }

    public virtual void OnGotHarmful(Mobile harmer, Mobile harmed)
    {
        Parent?.OnGotHarmful(harmer, harmed);
    }

    public virtual void OnLocationChanged(Mobile m, Point3D oldLocation)
    {
        Parent?.OnLocationChanged(m, oldLocation);
    }

    public virtual bool OnTarget(Mobile m, Target t, object o) => Parent?.OnTarget(m, t, o) != false;

    public virtual bool OnCombatantChange(Mobile m, Mobile old, Mobile newMobile) =>
        Parent?.OnCombatantChange(m, old, newMobile) != false;

    public virtual bool AllowHousing(Mobile from, Point3D p) => Parent?.AllowHousing(from, p) != false;

    public virtual bool SendInaccessibleMessage(Item item, Mobile from) =>
        Parent?.SendInaccessibleMessage(item, from) == true;

    public virtual bool CheckAccessibility(Item item, Mobile from) => Parent?.CheckAccessibility(item, from) != false;

    public virtual bool OnDecay(Item item) => Parent?.OnDecay(item) != false;

    public virtual bool AllowHarmful(Mobile from, Mobile target) =>
        Parent?.AllowHarmful(from, target) ?? Mobile.AllowHarmfulHandler?.Invoke(from, target) ?? true;

    public virtual void OnCriminalAction(Mobile m, bool message)
    {
        if (Parent != null)
        {
            Parent.OnCriminalAction(m, message);
        }
        else if (message)
        {
            m.SendLocalizedMessage(1005040); // You've committed a criminal act!!
        }
    }

    public virtual bool AllowBeneficial(Mobile from, Mobile target) =>
        Parent?.AllowBeneficial(from, target) ??
        Mobile.AllowBeneficialHandler?.Invoke(from, target) ?? true;

    public virtual void OnBeneficialAction(Mobile helper, Mobile target)
    {
        Parent?.OnBeneficialAction(helper, target);
    }

    public virtual void OnGotBeneficialAction(Mobile helper, Mobile target)
    {
        Parent?.OnGotBeneficialAction(helper, target);
    }

    public virtual void SpellDamageScalar(Mobile caster, Mobile target, ref double damage)
    {
        Parent?.SpellDamageScalar(caster, target, ref damage);
    }

    public virtual void OnSpeech(SpeechEventArgs args)
    {
        Parent?.OnSpeech(args);
    }

    public virtual bool AllowGain(Mobile m, Skill skill, object obj) => Parent?.AllowGain(m, skill, obj) ?? true;

    public virtual bool OnSkillUse(Mobile m, int skill) => Parent?.OnSkillUse(m, skill) ?? true;

    public virtual bool OnBeginSpellCast(Mobile m, ISpell s) => Parent?.OnBeginSpellCast(m, s) ?? true;

    public virtual void OnSpellCast(Mobile m, ISpell s)
    {
        Parent?.OnSpellCast(m, s);
    }

    public virtual bool OnResurrect(Mobile m) => Parent?.OnResurrect(m) ?? true;

    public virtual bool OnBeforeDeath(Mobile m) => Parent?.OnBeforeDeath(m) ?? true;

    public virtual void OnDeath(Mobile m)
    {
        Parent?.OnDeath(m);
    }

    public virtual bool OnDamage(Mobile m, ref int damage) => Parent?.OnDamage(m, ref damage) ?? true;

    public virtual bool OnHeal(Mobile m, ref int heal) => Parent?.OnHeal(m, ref heal) ?? true;

    public virtual bool OnDoubleClick(Mobile m, object o) => Parent?.OnDoubleClick(m, o) ?? true;

    public virtual bool OnSingleClick(Mobile m, object o) => Parent?.OnSingleClick(m, o) ?? true;

    public virtual bool AllowSpawn() => Parent?.AllowSpawn() ?? true;

    public virtual void AlterLightLevel(Mobile m, ref int global, ref int personal)
    {
        Parent?.AlterLightLevel(m, ref global, ref personal);
    }

    public virtual TimeSpan GetLogoutDelay(Mobile m)
    {
        if (Parent != null)
        {
            return Parent.GetLogoutDelay(m);
        }

        return m.AccessLevel > AccessLevel.Player ? StaffLogoutDelay : DefaultLogoutDelay;
    }

    internal static bool CanMove(Mobile m, Direction d, Point3D newLocation, Point3D oldLocation, Map map)
    {
        var oldRegion = m.Region;
        var newRegion = Find(newLocation, map);

        while (oldRegion != newRegion)
        {
            if (!newRegion.OnMoveInto(m, d, newLocation, oldLocation))
            {
                return false;
            }

            if (newRegion.Parent == null)
            {
                return true;
            }

            newRegion = newRegion.Parent;
        }

        return true;
    }

    internal static void OnRegionChange(Mobile m, Region oldRegion, Region newRegion)
    {
        if (newRegion != null && m.NetState != null)
        {
            m.CheckLightLevels(false);

            if (oldRegion == null || oldRegion.Music != newRegion.Music)
            {
                m.NetState.SendPlayMusic(newRegion.Music);
            }
        }

        var oldR = oldRegion;
        var newR = newRegion;

        while (oldR != newR)
        {
            var oldRChild = oldR?.ChildLevel ?? -1;
            var newRChild = newR?.ChildLevel ?? -1;

            if (oldRChild >= newRChild)
            {
                oldR?.OnExit(m);
                oldR = oldR?.Parent;
            }

            if (newRChild >= oldRChild)
            {
                newR?.OnEnter(m);
                newR = newR?.Parent;
            }
        }
    }
}
