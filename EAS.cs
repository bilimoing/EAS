using System.IO;
using EAS.Common.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace EAS;

public class EAS : Mod
{
    private const byte ExtraArmorSlotPacket = 1;
    private const byte ExtraArmorVisibilityPacket = 2;

    public override void Load()
    {
        On_Player.PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool += On_PlayerOnPickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool;
    }

    private void On_PlayerOnPickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool(On_Player.orig_PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool orig, Player self, Item sItem, ref int projToShoot, ref float speed, ref bool canShoot, ref int totalDamage, ref float knockBack, out int usedAmmoItemId, bool dontConsume)
    {
        orig(self, sItem, ref projToShoot, ref speed, ref canShoot, ref totalDamage, ref knockBack, out usedAmmoItemId, dontConsume);

        if (self.GetModPlayer<AccPlayer>().HasCursedQuiver && projToShoot == 1)
        {
            projToShoot = 103;
            sItem.damage += 2;
        }

        if (self.GetModPlayer<AccPlayer>().HasIchorQuiver && projToShoot == 1)
        {
            projToShoot = 278;
            sItem.damage += 2;
        }

        if (self.GetModPlayer<AccPlayer>().HasFrozenQuiver && projToShoot == 1)
        {
            projToShoot = 172;
            sItem.damage += 2;
        }

        if (self.GetModPlayer<AccPlayer>().HasVenomQuiver && projToShoot == 1)
        {
            projToShoot = 282;
            sItem.damage += 2;
        }
    }

    public override void HandlePacket(BinaryReader reader, int whoAmI)
    {
        switch (reader.ReadByte())
        {
            case ExtraArmorSlotPacket:
            {
                int playerIndex = Main.netMode == NetmodeID.Server ? whoAmI : reader.ReadByte();
                byte kind = reader.ReadByte();
                int slot = reader.ReadByte();
                Item item = ItemIO.Receive(reader, readStack: true);
                if (playerIndex is >= 0 and < Main.maxPlayers)
                {
                    Main.player[playerIndex].GetModPlayer<ExtraArmorPlayer>().ReceiveSlot(kind, slot, item);
                    if (Main.netMode == NetmodeID.Server)
                    {
                        SendExtraArmorSlot(-1, kind, slot, item, playerIndex);
                    }
                }

                break;
            }
            case ExtraArmorVisibilityPacket:
            {
                int playerIndex = Main.netMode == NetmodeID.Server ? whoAmI : reader.ReadByte();
                int slot = reader.ReadByte();
                bool hidden = reader.ReadBoolean();
                if (playerIndex is >= 0 and < Main.maxPlayers && slot < ExtraArmorPlayer.SlotCount)
                {
                    Main.player[playerIndex].GetModPlayer<ExtraArmorPlayer>().HideVisual[slot] = hidden;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        SendExtraArmorVisibility(-1, slot, hidden, playerIndex);
                    }
                }

                break;
            }
        }
    }

    internal static void SendExtraArmorSlot(int toWho, byte kind, int slot, Item item, int player = -1)
    {
        if (Main.netMode != NetmodeID.SinglePlayer)
        {
            int playerIndex = player >= 0 ? player : Main.myPlayer;
            ModPacket packet = ModContent.GetInstance<EAS>().GetPacket();
            packet.Write(ExtraArmorSlotPacket);
            if (Main.netMode == NetmodeID.Server)
            {
                packet.Write((byte)playerIndex);
            }

            packet.Write(kind);
            packet.Write((byte)slot);
            ItemIO.Send(item ?? new Item(), packet, writeStack: true);
            packet.Send(toWho, Main.netMode == NetmodeID.MultiplayerClient ? Main.myPlayer : playerIndex);
        }
    }

    internal static void SendExtraArmorVisibility(int toWho, int slot, bool hidden, int player = -1)
    {
        if (Main.netMode != NetmodeID.SinglePlayer)
        {
            int playerIndex = player >= 0 ? player : Main.myPlayer;
            ModPacket packet = ModContent.GetInstance<EAS>().GetPacket();
            packet.Write(ExtraArmorVisibilityPacket);
            if (Main.netMode == NetmodeID.Server)
            {
                packet.Write((byte)playerIndex);
            }

            packet.Write((byte)slot);
            packet.Write(hidden);
            packet.Send(toWho, Main.netMode == NetmodeID.MultiplayerClient ? Main.myPlayer : playerIndex);
        }
    }

    public override void Unload()
    {
        On_Player.PickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool -= On_PlayerOnPickAmmo_Item_refInt32_refSingle_refBoolean_refInt32_refSingle_refInt32_bool;
    }
}