using EAS.Common.Players;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace EAS.Content.Items.Accessories;

[AutoloadEquip(EquipType.Back)]
public class CursedQuiver : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }
    
    public override void SetDefaults()
    {
        Item.DefaultToAccessory(34, 36);
        Item.SetShopValues(ItemRarityColor.Pink5, Item.sellPrice(0, 7, 50));
    }
    
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.magicQuiver = true;
        player.arrowDamage += 0.1f;
        player.GetModPlayer<AccPlayer>().HasCursedQuiver = true;
    }
    
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.MagicQuiver)
            .AddIngredient<CurseStone>()
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}