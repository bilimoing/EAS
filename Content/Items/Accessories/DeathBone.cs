using EAS.Common.Players;
using EAS.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace EAS.Content.Items.Accessories;

public class DeathBone : ModItem
{
    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 1;
    }

    public override void SetDefaults()
    {
        Item.width = 34;
        Item.height = 28;
        Item.value = Item.sellPrice(0, 2);
        Item.accessory = true;
        Item.rare = ItemRarityID.Master;
        Item.master = true;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.GetModPlayer<AccPlayer>().DeathBone = true;

        if (Main.netMode != NetmodeID.MultiplayerClient)
        {
            int handType = ModContent.ProjectileType<SkeletonHand>();
            int desiredCount = Main.dayTime ? 2 : 4;
            var hands = new Projectile[Main.maxProjectiles];
            int handCount = 0;

            foreach (Projectile projectile in Main.ActiveProjectiles)
            {
                if (projectile.owner == player.whoAmI && projectile.type == handType && handCount < hands.Length)
                    hands[handCount++] = projectile;
            }

            int keepCount = handCount < desiredCount ? handCount : desiredCount;
            for (int index = 0; index < keepCount; index++)
            {
                hands[index].ai[0] = index;
                hands[index].ai[1] = desiredCount;
                hands[index].netUpdate = true;
            }

            for (int index = desiredCount; index < handCount; index++)
            {
                hands[index].Kill();
            }

            if (handCount < desiredCount)
            {
                IEntitySource source = player.GetSource_Accessory(Item);
                for (int index = handCount; index < desiredCount; index++)
                {
                    Projectile.NewProjectile(source, player.Center, Vector2.Zero, handType, 28, 4f,
                        player.whoAmI, index, desiredCount);
                }
            }
        }
    }
}
