using EAS.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EAS.Content.Items.Accessories;

[AutoloadEquip(EquipType.HandsOff, EquipType.HandsOn)]
public class FuzzyHandcuffs : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }
    
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.accessory = true;
        Item.value = Item.sellPrice(0,2);
        Item.rare = ItemRarityID.Blue;
    }
    
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<AccPlayer>().FuzzyHandcuffs = true;
        player.whipRangeMultiplier *= 1.3f;
        player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) *= 1.15f;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.FlinxFur,2)
            .AddIngredient(ItemID.Shackle)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}