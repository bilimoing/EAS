using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EAS.Content.Items.Accessories;

public class TerrapinEmblem : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 36;
        Item.rare = ItemRarityID.Yellow;
        Item.accessory = true;
        Item.value = 10000;
    }
    
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetDamage(DamageClass.Melee) += 0.2f;
    }
    
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ItemID.WarriorEmblem)
            .AddIngredient(ItemID.TurtleShell)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}