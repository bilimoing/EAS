using EAS.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EAS.Content.Items.Accessories;

[AutoloadEquip(EquipType.HandsOff, EquipType.HandsOn)]
public class FrozenGloves : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.rare = ItemRarityID.Lime;
        Item.value = 300000;
        Item.accessory = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.kbGlove = true;
        player.autoReuseGlove = true;
        player.meleeScaleGlove = true;
        player.GetAttackSpeed(DamageClass.Melee) += 0.12f;
        player.GetDamage(DamageClass.Melee) += 0.12f;
        player.GetModPlayer<AccPlayer>().FrozenStone = true;
    }

    public override void AddRecipes()
    {
        Recipe modRecipe = Recipe.Create(Type);
        modRecipe.AddIngredient(ItemID.MechanicalGlove);
        modRecipe.AddIngredient(ModContent.ItemType<FrozenStone>());
        modRecipe.AddTile(TileID.TinkerersWorkbench);
        modRecipe.Register();
    }
}