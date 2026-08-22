using System;
using ModernUO.Serialization;
using Server.Engines.Craft;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Items;

[SerializationGenerator(2, false)]
public abstract partial class BaseOre : Item
{
    public BaseOre(CraftResource resource, int amount = 1) : base(0x19B9)
    {
        Stackable = true;
        Amount = amount;
        Hue = CraftResources.GetHue(resource);

        _resource = resource;
    }

    [SerializableProperty(0)]
    [CommandProperty(AccessLevel.GameMaster)]
    public CraftResource Resource
    {
        get => _resource;
        set
        {
            if (_resource != value)
            {
                _resource = value;
                Hue = CraftResources.GetHue(value);

                InvalidateProperties();
                this.MarkDirty();
            }
        }
    }

    public override int LabelNumber
    {
        get
        {
            if (_resource is >= CraftResource.DullCopper and <= CraftResource.Valorite)
            {
                return 1042845 + (_resource - CraftResource.DullCopper);
            }

            return 1042853; // iron ore;
        }
    }

    public abstract BaseIngot GetIngot();

    private void Deserialize(IGenericReader reader, int version)
    {
        // Use this line instead if you are getting world loading issues
        _resource = (CraftResource)reader.ReadByte();
        // _resource = (CraftResource)reader.ReadInt();
    }

    private static int RandomSize() =>
        Utility.RandomDouble() switch
        {
            < 0.125  => 0x19B7, // Small
            < 0.1875 => 0x19B8, // Medium clump
            < 0.25   => 0x19BA, // Medium
            _        => 0x19B9  // Large
        };

    public override bool CanStackWith(Item dropped) =>
        base.CanStackWith(dropped) &&
        (dropped as BaseOre)?._resource == _resource;

    public override void AddNameProperty(IPropertyList list)
    {
        if (Amount > 1)
        {
            list.Add(1050039, $"{Amount}\t{1026583:#}"); // ~1_NUMBER~ ~2_ITEMNAME~
        }
        else
        {
            list.Add(1026583); // ore
        }
    }

    public override void GetProperties(IPropertyList list)
    {
        base.GetProperties(list);

        if (!CraftResources.IsStandard(_resource))
        {
            var num = CraftResources.GetLocalizationNumber(_resource);

            if (num > 0)
            {
                list.Add(num);
            }
            else
            {
                list.Add(CraftResources.GetName(_resource));
            }
        }
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!Movable)
        {
            return;
        }

        if (RootParent is BaseCreature)
        {
            from.SendLocalizedMessage(500447); // That is not accessible
        }
        else if (from.InRange(GetWorldLocation(), 2))
        {
            // Select the forge on which to smelt the ore, or another pile of ore with which to combine it.
            from.SendLocalizedMessage(501971);
            from.Target = new InternalTarget(this);
        }
        else
        {
            from.SendLocalizedMessage(501976); // The ore is too far away.
        }
    }

    private class InternalTarget : Target
    {
        private readonly BaseOre m_Ore;

        public InternalTarget(BaseOre ore) : base(2, false, TargetFlags.None) => m_Ore = ore;

        private bool IsForge(object obj)
        {
            if (Core.ML && obj is Mobile { IsDeadBondedPet: true })
            {
                return false;
            }

            if (obj.GetType().IsDefined(typeof(ForgeAttribute), false))
            {
                return true;
            }

            var itemID = obj switch
            {
                Item item           => item.ItemID,
                StaticTarget target => target.ItemID,
                _                   => 0
            };

            return itemID is 4017 or >= 6522 and <= 6569 or 11736;
        }

        protected override void OnTarget(Mobile from, object targeted)
        {
            if (m_Ore.Deleted)
            {
                return;
            }

            if (!from.InRange(m_Ore.GetWorldLocation(), 2))
            {
                from.SendLocalizedMessage(501976); // The ore is too far away.
                return;
            }

            if (targeted is BaseOre ore)
            {
                OnTargetOre(from, ore);
                return;
            }

            if (!IsForge(targeted))
            {
                return;
            }

            var difficulty = m_Ore._resource switch
            {
                CraftResource.DullCopper => 65.0,
                CraftResource.ShadowIron => 70.0,
                CraftResource.Copper     => 75.0,
                CraftResource.Bronze     => 80.0,
                CraftResource.Gold       => 85.0,
                CraftResource.Agapite    => 90.0,
                CraftResource.Verite     => 95.0,
                CraftResource.Valorite   => 99.0,
                _                        => 50.0
            };

            if (difficulty > 50.0 && difficulty > from.Skills.Mining.Value)
            {
                from.SendLocalizedMessage(501986); // You have no idea how to smelt this strange ore!
                return;
            }

            if (!from.BeginAction<BaseOre>())
            {
                from.SendLocalizedMessage(500119); // You must wait to perform another action
                return;
            }

            SmeltTick(from, targeted, difficulty - 25.0, difficulty + 25.0);
        }

        private void SmeltTick(Mobile from, object forge, double minSkill, double maxSkill)
        {
            bool canSmelt = true;

            if (!from.InRange(m_Ore.GetWorldLocation(), 2))
            {
                canSmelt = false;
                from.SendLocalizedMessage(501976); // The ore is too far away.
            }

            if (!from.InLOS(forge))
            {
                canSmelt = false;
                OnTargetOutOfLOS(from, forge);
            }

            if (!from.InRange(GetLocation(forge), 2))
            {
                canSmelt = false;
                OnTargetOutOfRange(from, forge);
            }

            if (!canSmelt || !from.Alive || from.NetState == null || m_Ore.Deleted || m_Ore.Amount <= 0)
            {
                from.EndAction<BaseOre>();
                return;
            }

            if (from.CheckTargetSkill(SkillName.Mining, forge, minSkill, maxSkill))
            {
                var ingot = m_Ore.GetIngot();
                ingot.Amount = 2;

                m_Ore.Consume(1);
                from.AddToBackpack(ingot);
                from.PlaySound(0x57);

                // You smelt the ore removing the impurities and put the metal in your backpack.
                from.SendLocalizedMessage(501988);
            }
            else
            {
                m_Ore.Consume(1);

                // You burn away the impurities but are left with less useable metal.
                from.SendLocalizedMessage(501990);
            }

            if (m_Ore.Amount <= 0)
            {
                from.EndAction<BaseOre>();
                return;
            }

            Timer.StartTimer(TimeSpan.FromMilliseconds(250), () => SmeltTick(from, forge, minSkill, maxSkill));
        }

        private static Point3D GetLocation(object obj) => obj switch
        {
            Item item            => item.GetWorldLocation(),
            StaticTarget target  => target.Location,
            LandTarget target    => target.Location,
            Mobile mobile        => mobile.Location,
            _                    => Point3D.Zero
        };

        private void OnTargetOre(Mobile from, BaseOre ore)
        {
            if (!ore.Movable)
            {
                return;
            }

            if (m_Ore == ore)
            {
                from.SendLocalizedMessage(501972); // Select another pile or ore with which to combine this.
                from.Target = new InternalTarget(ore);
                return;
            }

            if (ore._resource != m_Ore._resource)
            {
                from.SendLocalizedMessage(501979); // You cannot combine ores of different metals.
                return;
            }

            var worth = ore.Amount * ore.ItemID switch
            {
                0x19B9 => 8,
                0x19B7 => 2,
                _      => 4
            };

            var sourceWorth = m_Ore.Amount * m_Ore.ItemID switch
            {
                0x19B9 => 8,
                0x19B7 => 2,
                _      => 4
            };

            worth += sourceWorth;

            var plusWeight = 0;
            var newID = ore.ItemID;

            if (ore.DefaultWeight != m_Ore.DefaultWeight)
            {
                if (ore.ItemID == 0x19B7 || m_Ore.ItemID == 0x19B7)
                {
                    newID = 0x19B7;
                }
                else if (ore.ItemID == 0x19B9)
                {
                    newID = m_Ore.ItemID;
                    plusWeight = ore.Amount * 2;
                }
                else
                {
                    plusWeight = m_Ore.Amount * 2;
                }
            }

            if (ore.ItemID == 0x19B9 && worth > 120000 ||
                ore.ItemID is 0x19B8 or 0x19BA && worth > 60000 ||
                ore.ItemID == 0x19B7 && worth > 30000)
            {
                from.SendLocalizedMessage(1062844); // There is too much ore to combine.
                return;
            }

            if (ore.RootParent is Mobile mobile &&
                plusWeight + mobile.Backpack.TotalWeight > mobile.Backpack.MaxWeight)
            {
                from.SendLocalizedMessage(501978); // The weight is too great to combine in a container.
                return;
            }

            ore.ItemID = newID;

            ore.Amount = ore.ItemID switch
            {
                0x19B9 => worth / 8,
                0x19B7 => worth / 2,
                _      => worth / 4
            };

            m_Ore.Delete();
        }
    }
}

[SerializationGenerator(0, false)]
public partial class IronOre : BaseOre
{
    [Constructible]
    public IronOre(int amount = 1) : base(CraftResource.Iron, amount)
    {
    }

    public IronOre(bool fixedSize) : this()
    {
        if (fixedSize)
        {
            ItemID = 0x19B8;
        }
    }

    public override BaseIngot GetIngot() => new IronIngot();
}

[SerializationGenerator(0, false)]
public partial class DullCopperOre : BaseOre
{
    [Constructible]
    public DullCopperOre(int amount = 1) : base(CraftResource.DullCopper, amount)
    {
    }

    public override BaseIngot GetIngot() => new DullCopperIngot();
}

[SerializationGenerator(0, false)]
public partial class ShadowIronOre : BaseOre
{
    [Constructible]
    public ShadowIronOre(int amount = 1) : base(CraftResource.ShadowIron, amount)
    {
    }

    public override BaseIngot GetIngot() => new ShadowIronIngot();
}

[SerializationGenerator(0, false)]
public partial class CopperOre : BaseOre
{
    [Constructible]
    public CopperOre(int amount = 1) : base(CraftResource.Copper, amount)
    {
    }

    public override BaseIngot GetIngot() => new CopperIngot();
}

[SerializationGenerator(0, false)]
public partial class BronzeOre : BaseOre
{
    [Constructible]
    public BronzeOre(int amount = 1) : base(CraftResource.Bronze, amount)
    {
    }

    public override BaseIngot GetIngot() => new BronzeIngot();
}

[SerializationGenerator(0, false)]
public partial class GoldOre : BaseOre
{
    [Constructible]
    public GoldOre(int amount = 1) : base(CraftResource.Gold, amount)
    {
    }

    public override BaseIngot GetIngot() => new GoldIngot();
}

[SerializationGenerator(0, false)]
public partial class AgapiteOre : BaseOre
{
    [Constructible]
    public AgapiteOre(int amount = 1) : base(CraftResource.Agapite, amount)
    {
    }

    public override BaseIngot GetIngot() => new AgapiteIngot();
}

[SerializationGenerator(0, false)]
public partial class VeriteOre : BaseOre
{
    [Constructible]
    public VeriteOre(int amount = 1) : base(CraftResource.Verite, amount)
    {
    }

    public override BaseIngot GetIngot() => new VeriteIngot();
}

[SerializationGenerator(0, false)]
public partial class ValoriteOre : BaseOre
{
    [Constructible]
    public ValoriteOre(int amount = 1) : base(CraftResource.Valorite, amount)
    {
    }

    public override BaseIngot GetIngot() => new ValoriteIngot();
}
