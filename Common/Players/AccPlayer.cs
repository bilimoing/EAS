using EAS.Content.Items.Accessories;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EAS.Common.Players;

public class AccPlayer : ModPlayer
{
    public bool FrozenStone;
    public bool IchorStone;
    public bool CurseStone;
    public bool VenomStone;
    public bool HasCursedQuiver;
    public bool HasIchorQuiver;
    public bool HasFrozenQuiver;
    public bool HasVenomQuiver;
    public bool DeathBone;
    public bool ObsidianScarf;
    public int DashDir = -1;
    public bool DashActive;
    public int DashDelay = MAX_DASH_DELAY;
    public int DashTimer = MAX_DASH_TIMER;
    public readonly float DashVelocity = 15f;
    public static readonly int MAX_DASH_DELAY = 10;
    public static readonly int MAX_DASH_TIMER = 10;
    public static readonly int DashDown = 0;
    public static readonly int DashUp = 1;
    public static readonly int DashRight = 2;
    public static readonly int DashLeft = 3;

    public override void ResetEffects()
    {
        FrozenStone = false;
        IchorStone = false;
        CurseStone = false;
        VenomStone = false;
        HasCursedQuiver = false;
        HasIchorQuiver = false;
        HasFrozenQuiver = false;
        HasVenomQuiver = false;
        DeathBone = false;
        ObsidianScarf = false;
        bool dashAccessoryEquipped = false;
        for (int i = 3; i < 8 + Player.extraAccessorySlots; i++)
        {
            Item item = Player.armor[i];
            if (item.type == ModContent.ItemType<ObsidianScarf>())
            {
                dashAccessoryEquipped = true;
            }
            else if (item.type is ItemID.EoCShield or ItemID.MasterNinjaGear or ItemID.Tabi)
            {
                return;
            }
        }

        if (!dashAccessoryEquipped || Player.setSolar || Player.mount.Active || DashActive)
        {
            return;
        }

        if (Player.controlDown && Player.releaseDown && Player.doubleTapCardinalTimer[DashDown] < 15)
        {
            DashDir = DashDown;
        }
        else if (Player.controlUp && Player.releaseUp && Player.doubleTapCardinalTimer[DashUp] < 15)
        {
            DashDir = DashUp;
        }
        else if (Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[DashRight] < 15)
        {
            DashDir = DashRight;
        }
        else
        {
            if (!Player.controlLeft || !Player.releaseLeft || Player.doubleTapCardinalTimer[DashLeft] >= 15)
            {
                return;
            }

            DashDir = DashLeft;
        }

        DashActive = true;
    }

    public override void UpdateDead()
    {
        FrozenStone = false;
        IchorStone = false;
        CurseStone = false;
        VenomStone = false;
        HasCursedQuiver = false;
        HasIchorQuiver = false;
        HasVenomQuiver = false;
        HasFrozenQuiver = false;
        DeathBone = false;
        ObsidianScarf = false;
    }

    public override void OnHitNPCWithItem(Item item, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (FrozenStone && item.CountsAsClass(DamageClass.Melee))
        {
            target.AddBuff(BuffID.Frostburn, 600);
        }

        if (IchorStone && item.CountsAsClass(DamageClass.Melee))
        {
            target.AddBuff(BuffID.Ichor, 600);
        }

        if (CurseStone && item.CountsAsClass(DamageClass.Melee))
        {
            target.AddBuff(BuffID.CursedInferno, 600);
        }

        if (VenomStone && item.CountsAsClass(DamageClass.Melee))
        {
            target.AddBuff(BuffID.Venom, 600);
        }
    }

    public override void OnHitNPCWithProj(Projectile proj, NPC target, NPC.HitInfo hit, int damageDone)
    {
        if (FrozenStone && proj.CountsAsClass(DamageClass.Melee))
        {
            target.AddBuff(BuffID.Frostburn, 600);
        }

        if (IchorStone && proj.CountsAsClass(DamageClass.Melee))
        {
            target.AddBuff(BuffID.Ichor, 600);
        }

        if (VenomStone && proj.CountsAsClass(DamageClass.Melee))
        {
            target.AddBuff(BuffID.Venom, 600);
        }

        if (CurseStone && proj.CountsAsClass(DamageClass.Melee))
        {
            target.AddBuff(BuffID.CursedInferno, 600);
        }
    }
}