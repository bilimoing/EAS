using EAS.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EAS.Content.Items.Accessories;

public class BottledShimmer : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.accessory = true;
        Item.value = 10000;
        Item.rare = ItemRarityID.Blue;
    }
    
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<AccPlayer>().BottledShimmer = true;
    }
    
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }
    
    public override void AddRecipes()
    {
        Recipe.Create(Type)
            .AddIngredient(ItemID.Bottle)
            .AddIngredient(ItemID.FallenStar)
            .AddIngredient(ItemID.Shimmerfly)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }

}