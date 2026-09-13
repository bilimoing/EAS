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
    public bool LifeFlower;
    public bool OilBarrel;
    public bool BoilingWaterBottle;
    public bool BottledShimmer;
    public bool FuzzyHandcuffs;
    public int DashDir = -1;
    public bool DashActive;
    public int DashDelay = MAX_DASH_DELAY;
    public int DashTimer = MAX_DASH_TIMER;
    public const float DashVelocity = 15f;
    public const int MAX_DASH_DELAY = 10;
    public const int MAX_DASH_TIMER = 10;
    public const int DashDown = 0;
    public const int DashUp = 1;
    public const int DashRight = 2;
    public const int DashLeft = 3;

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
        LifeFlower = false;
        OilBarrel = false;
        BoilingWaterBottle = false;
        BottledShimmer = false;
        FuzzyHandcuffs = false;
        
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
        LifeFlower = false;
        BoilingWaterBottle = false;
        OilBarrel = false;
        BottledShimmer = false;
        FuzzyHandcuffs = false;
    }
    
    public override void ModifyLuck(ref float luck)
    {
        if (BottledShimmer)
        {
            luck += 0.03f; 
        }
    }
    
    public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref NPC.HitModifiers modifiers)
    {
        if (BottledShimmer)
        {
            if (Player.ZoneShimmer)
            {
                modifiers.FinalDamage *= 1.08f;
            }
        }
    }

    public override void ModifyHitNPCWithItem(Item item, NPC target, ref NPC.HitModifiers modifiers)
    {
        if (BottledShimmer)
        {
            if (Player.ZoneShimmer)
            {
                modifiers.FinalDamage *= 1.08f;
            }
        }
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
        
        if (OilBarrel && item.CountsAsClass(DamageClass.Melee))
        {
            target.AddBuff(BuffID.Oiled, 240);
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
        
        if (OilBarrel && proj.CountsAsClass(DamageClass.Melee))
        {
            target.AddBuff(BuffID.Oiled, 180);
        }
        if (OilBarrel && proj.CountsAsClass(DamageClass.Ranged))
        {
            target.AddBuff(BuffID.Oiled, 180);
        }
    }
    
    public override void PostHurt(Player.HurtInfo info)
    {
        if (LifeFlower && Player.QuickHeal_GetItemToUse() != null && Player.statLife + Player.QuickHeal_GetItemToUse().healLife < Player.statLifeMax2)
        {
            Player.QuickHeal();
        }
    }

    public override void PostUpdate()
    {
        if (BoilingWaterBottle)
        {
            Dust.NewDust(Player.position, Player.width, Player.height, DustID.Water, 0f, 0f, 100);
            bool isSubmerged = Player.wet;
            if (isSubmerged)
            {
                Player.moveSpeed += 0.2f;
                Player.statDefense += 5;
                Player.breathMax += 30;
            }
        }
    }
}