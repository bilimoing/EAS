using EAS.Common.Players;
using Terraria;
using Terraria.ModLoader;

namespace EAS;

public class EAS : Mod
{
    public override void Load()
    {
        On_Player.PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool += On_PlayerOnPickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool;
    }

    private void On_PlayerOnPickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool(On_Player.orig_PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool orig, Player self, Item sItem, ref int projToShoot, ref float speed, ref bool canShoot, ref int totalDamage, ref float knockBack, out int usedAmmoItemId, bool dontConsume)
    {
        orig(self, sItem, ref projToShoot, ref speed, ref canShoot, ref totalDamage, ref knockBack, out usedAmmoItemId, dontConsume);
        
        if (self.GetModPlayer<AccPlayer>().HasCursedQuiver && projToShoot == 1)
        {
            projToShoot = 103;
            sItem.damage += 2;
        }
        
        if (self.GetModPlayer<AccPlayer>().HasIchorQuiver && projToShoot == 1)
        {
            projToShoot = 278;
            sItem.damage += 2;
        }
        
        if (self.GetModPlayer<AccPlayer>().HasFrozenQuiver && projToShoot == 1)
        {
            projToShoot = 172;
            sItem.damage += 2;
        }
        
        if (self.GetModPlayer<AccPlayer>().HasVenomQuiver && projToShoot == 1)
        {
            projToShoot = 282;
            sItem.damage += 2;
        }
    }
    
    public override void Unload()
    {
        On_Player.PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool -= On_PlayerOnPickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool;
    }
}