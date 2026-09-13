using EAS.Common.Players;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EAS.Content.Items.Accessories;

public class OilBarrel : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
        Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(20, 7));
        ItemID.Sets.AnimatesAsSoul[Item.type] = true;
    }


    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 36;
        Item.rare = ItemRarityID.Lime;
        Item.accessory = true;
        Item.value = Item.sellPrice(0, 4, 5);
    }

    public override void AddRecipes()
    {
        Recipe modRecipe = Recipe.Create(Type);
        modRecipe.AddIngredient(ItemID.SoulofFright, 10);
        modRecipe.AddIngredient(ItemID.TurtleShell);
        modRecipe.AddIngredient(ItemID.ChlorophyteBar, 5);
        modRecipe.AddTile(TileID.MythrilAnvil);
        modRecipe.Register();
    }


    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<AccPlayer>().OilBarrel = true;
    }
}