using EAS.Common.Players;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;

namespace EAS.Content.Items.Accessories;

[AutoloadEquip(EquipType.Back, EquipType.Front)]
public class LifeCloak : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }
    
    public override void SetDefaults()
    {
        Item.DefaultToAccessory(26, 36);
        Item.SetShopValues(ItemRarityColor.Pink5, Item.sellPrice(0, 3));
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<AccPlayer>().LifeFlower = true;   
        player.starCloakItem = Item;
        player.starCloakItem_manaCloakOverrideItem = Item;
    }
    
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<LifeFlower>()
            .AddIngredient(ItemID.StarCloak)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}