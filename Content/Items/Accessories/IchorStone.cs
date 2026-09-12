using EAS.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace EAS.Content.Items.Accessories;

public class IchorStone : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 28;
        Item.rare = ItemRarityID.Orange;
        Item.accessory = true;
        Item.value = Item.sellPrice(0, 2);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<AccPlayer>().IchorStone = true;
    }
}