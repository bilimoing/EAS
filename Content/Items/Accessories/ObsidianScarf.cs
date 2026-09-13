using EAS.Common.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace EAS.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Neck)]
    public class ObsidianScarf : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
        }

        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 28;
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<AccPlayer>().ObsidianScarf = true;
            AccPlayer mp = player.GetModPlayer<AccPlayer>();
            if (!mp.DashActive)
            {
                return;
            }
            player.eocDash = mp.DashTimer;
            player.armorEffectDrawShadowEOCShield = true;
            if (mp.DashTimer == AccPlayer.MAX_DASH_TIMER)
            {
                Vector2 newVelocity = player.velocity;
                if ((mp.DashDir == AccPlayer.DashUp && player.velocity.Y > -mp.DashVelocity) || (mp.DashDir == AccPlayer.DashDown && player.velocity.Y < mp.DashVelocity))
                {
                    float dashDirection = (mp.DashDir == AccPlayer.DashDown) ? 1f : -1.3f;
                    newVelocity.Y = dashDirection * mp.DashVelocity;
                }
                else if ((mp.DashDir == AccPlayer.DashLeft && player.velocity.X > -mp.DashVelocity) || (mp.DashDir == AccPlayer.DashRight && player.velocity.X < mp.DashVelocity))
                {
                    int dashDirection2 = mp.DashDir == AccPlayer.DashRight ? 1 : -1;
                    newVelocity.X = dashDirection2 * mp.DashVelocity;
                }
                player.velocity = newVelocity;
            }
            mp.DashTimer--;
            mp.DashDelay--;
            if (mp.DashDelay == 0)
            {
                mp.DashDelay = AccPlayer.MAX_DASH_DELAY;
                mp.DashTimer = AccPlayer.MAX_DASH_TIMER;
                mp.DashActive = false;
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Obsidian, 10)
                .AddIngredient(ItemID.Silk, 3)
                .AddCondition(Condition.NearLava)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
}
