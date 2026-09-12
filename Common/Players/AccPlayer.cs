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