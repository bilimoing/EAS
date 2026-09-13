using EAS.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EAS.Content.Items.Accessories;

public class FlowerFlower : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 44;
        Item.accessory = true;
        Item.rare = ItemRarityID.LightRed;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.ManaFlower);
        recipe.AddIngredient(ModContent.ItemType<LifeFlower>());
        recipe.AddTile(TileID.TinkerersWorkbench);
        recipe.Register();
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {            
        player.manaFlower = true;
        player.endurance += 0.08f;
        player.GetModPlayer<AccPlayer>().LifeFlower = true;
        player.manaCost -= 0.08f;
    }
}