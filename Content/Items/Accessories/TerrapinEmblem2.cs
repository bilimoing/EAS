using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EAS.Content.Items.Accessories;

public class TerrapinEmblem2 : ModItem
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
        player.GetDamage(DamageClass.Melee) += 0.3f;
        if (player.statLife <= player.statLifeMax2 * 0.5)
        {
            player.AddBuff(BuffID.IceBarrier, 5);
        }
    }
    
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient<TerrapinEmblem>()
            .AddIngredient(ItemID.IceBlock, 200)
            .AddIngredient(ItemID.FrozenTurtleShell)
            .AddTile(TileID.TinkerersWorkbench)
            .Register();
    }
}