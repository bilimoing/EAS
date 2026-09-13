using EAS.Common.Players;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EAS.Content.Items.Accessories;

public class BoilingWaterBottle : ModItem
{
    public override void SetStaticDefaults()
    {
        ItemID.Sets.AnimatesAsSoul[Item.type] = true;
        Item.ResearchUnlockCount = 1;
        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(30, 2));
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 28;
        Item.accessory = true;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.sellPrice(0, 1);
    }

    public override void UpdateAccessory(Player player, bool isHidden)
    {
        player.GetModPlayer<AccPlayer>().BoilingWaterBottle = true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = Recipe.Create(Type);
        recipe.AddIngredient(ItemID.LavaBucket);
        recipe.AddIngredient(ItemID.BottledWater);
        recipe.AddTile(TileID.CookingPots);
        recipe.Register();
    }
}