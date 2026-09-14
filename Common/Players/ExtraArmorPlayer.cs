using System;
using System.Collections.Generic;
using System.Linq;
using EAS.Common.Configs;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace EAS.Common.Players;

public sealed class ExtraArmorPlayer : ModPlayer
{
    internal static int HoveredArmorType;

    public const int SlotCount = 3;
    public readonly Item[] Armor = CreateItems();
    public readonly Item[] Vanity = CreateItems();
    public readonly Item[] Dye = CreateItems();
    public readonly bool[] HideVisual = new bool[SlotCount];

    private readonly Item[,] loadoutArmor = new Item[3, SlotCount];
    private readonly Item[,] loadoutVanity = new Item[3, SlotCount];
    private readonly Item[,] loadoutDye = new Item[3, SlotCount];
    private readonly bool[,] loadoutHide = new bool[3, SlotCount];

    public override void Initialize()
    {
        EnsureArrays();
        EnsureLoadouts();
    }

    public override void ResetEffects() => EnsureArrays();

    public override void SaveData(TagCompound tag)
    {
        tag["ExtraArmor"] = SaveItems(Armor);
        tag["ExtraArmorVanity"] = SaveItems(Vanity);
        tag["ExtraArmorDye"] = SaveItems(Dye);
        tag["ExtraArmorHide"] = new List<bool>(HideVisual);

        for (int loadout = 0; loadout < 3; loadout++)
        {
            tag[$"ExtraArmor{loadout}"] = new TagCompound
            {
                ["armor"] = SaveItems(GetLoadoutItems(loadout, loadoutArmor)),
                ["vanity"] = SaveItems(GetLoadoutItems(loadout, loadoutVanity)),
                ["dye"] = SaveItems(GetLoadoutItems(loadout, loadoutDye)),
                ["hide"] = SaveHidden(loadout)
            };
        }
    }

    public override void LoadData(TagCompound tag)
    {
        EnsureArrays();
        EnsureLoadouts();
        LoadItems(tag, "ExtraArmor", Armor);
        LoadItems(tag, "ExtraArmorVanity", Vanity);
        LoadItems(tag, "ExtraArmorDye", Dye);
        LoadHidden(tag, "ExtraArmorHide", HideVisual);

        for (int loadout = 0; loadout < 3; loadout++) 
        {
            if (tag.ContainsKey($"ExtraArmor{loadout}"))
            {
                TagCompound saved = tag.GetCompound($"ExtraArmor{loadout}");
                LoadItems(saved, "armor", loadout, loadoutArmor);
                LoadItems(saved, "vanity", loadout, loadoutVanity);
                LoadItems(saved, "dye", loadout, loadoutDye);
                LoadHidden(saved, "hide", loadout, loadoutHide);
            }
        }
    }

    public override void FrameEffects()
    {
        if (ModContent.GetInstance<MyConfig>().Armor)
        {
            if (!Vanity[0].IsAir)
            {
                Player.head = Vanity[0].headSlot;
            }
            else if (!Armor[0].IsAir)
            {
                Player.head = Armor[0].headSlot;
            }

            if (!Vanity[1].IsAir)
            {
                Player.body = Vanity[1].bodySlot;
            }
            else if (!Armor[1].IsAir)
            {
                Player.body = Armor[1].bodySlot;
            }

            if (!Vanity[2].IsAir)
            {
                Player.legs = Vanity[2].legSlot;
            }
            else if (!Armor[2].IsAir)
            {
                Player.legs = Armor[2].legSlot;
            }

            if (!Dye[0].IsAir)
            {
                Player.cHead = Dye[0].dye;
            }

            if (!Dye[1].IsAir)
            {
                Player.cBody = Dye[1].dye;
            }

            if (!Dye[2].IsAir)
            {
                Player.cLegs = Dye[2].dye;
            }
        }
    }

    public override void ModifyDrawInfo(ref PlayerDrawSet drawInfo)
    {
        if (ModContent.GetInstance<MyConfig>().Armor)
        {
            drawInfo.drawPlayer.head = Player.head;
            drawInfo.drawPlayer.body = Player.body;
            drawInfo.drawPlayer.legs = Player.legs;
            drawInfo.cHead = Player.cHead;
            drawInfo.cBody = Player.cBody;
            drawInfo.cLegs = Player.cLegs;
        }
    }

    public override void UpdateEquips()
    {
        if (ModContent.GetInstance<MyConfig>().Armor)
        {
            for (int slot = 0; slot < SlotCount; slot++)
            {
                Item item = Armor[slot];
                if (IsAllowedForSlot(item, slot) && CanUseArmorItem(item))
                    Player.GrantArmorBenefits(item);
            }

            Item head = Armor[0];
            Item body = Armor[1];
            Item legs = Armor[2];

            ItemLoader.UpdateArmorSet(Player, head, body, legs);

            ArmorSetBonus.QueryContext context = new()
            {
                HeadItem = head.IsAir ? 0 : head.type,
                BodyItem = body.IsAir ? 0 : body.type,
                LegItem = legs.IsAir ? 0 : legs.type
            };
            ArmorSetBonuses.GetCompleteSet(context)?.Effect(Player);
        }
    }

    public override void CopyClientState(ModPlayer targetCopy)
    {
        ExtraArmorPlayer clone = (ExtraArmorPlayer)targetCopy;
        for (int i = 0; i < SlotCount; i++) 
        {
            Armor[i].CopyNetStateTo(clone.Armor[i]);
            Vanity[i].CopyNetStateTo(clone.Vanity[i]);
            Dye[i].CopyNetStateTo(clone.Dye[i]);
            clone.HideVisual[i] = HideVisual[i];
        }
    }

    public override void SendClientChanges(ModPlayer clientPlayer)
    {
        ExtraArmorPlayer old = (ExtraArmorPlayer)clientPlayer;
        for (int i = 0; i < SlotCount; i++) 
        {
            if (Armor[i].IsNetStateDifferent(old.Armor[i]))
            {
                EAS.SendExtraArmorSlot(Player.whoAmI, 0, i, Armor[i]);
            }

            if (Vanity[i].IsNetStateDifferent(old.Vanity[i]))
            {
                EAS.SendExtraArmorSlot(Player.whoAmI, 1, i, Vanity[i]);
            }

            if (Dye[i].IsNetStateDifferent(old.Dye[i]))
            {
                EAS.SendExtraArmorSlot(Player.whoAmI, 2, i, Dye[i]);
            }

            if (HideVisual[i] != old.HideVisual[i])
            {
                EAS.SendExtraArmorVisibility(Player.whoAmI, i, HideVisual[i]);
            }
        }
    }

    public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
    {
        for (int i = 0; i < SlotCount; i++) 
        {
            EAS.SendExtraArmorSlot(toWho, 0, i, Armor[i]);
            EAS.SendExtraArmorSlot(toWho, 1, i, Vanity[i]);
            EAS.SendExtraArmorSlot(toWho, 2, i, Dye[i]);
            EAS.SendExtraArmorVisibility(toWho, i, HideVisual[i]);
        }
    }

    public override void OnEquipmentLoadoutSwitched(int oldLoadoutIndex, int loadoutIndex)
    {
        EnsureLoadouts();
        if (oldLoadoutIndex is >= 0 and < 3 && loadoutIndex is >= 0 and < 3 && oldLoadoutIndex != loadoutIndex)
        {
            for (int slot = 0; slot < SlotCount; slot++)
            {
                loadoutArmor[oldLoadoutIndex, slot] = Armor[slot];
                loadoutVanity[oldLoadoutIndex, slot] = Vanity[slot];
                loadoutDye[oldLoadoutIndex, slot] = Dye[slot];
                loadoutHide[oldLoadoutIndex, slot] = HideVisual[slot];

                Armor[slot] = loadoutArmor[loadoutIndex, slot];
                Vanity[slot] = loadoutVanity[loadoutIndex, slot];
                Dye[slot] = loadoutDye[loadoutIndex, slot];
                HideVisual[slot] = loadoutHide[loadoutIndex, slot];
            }
        }
    }

    internal void ReceiveSlot(byte kind, int slot, Item item)
    {
        if (slot is >= 0 and < SlotCount)
        {
            if (kind == 0 && !IsAllowedForSlot(item, slot))
            {
                item = new Item();
            }

            if (kind == 1 && !IsAllowedForSlot(item, slot))
            {
                item = new Item();
            }

            if (kind == 2 && !IsDye(item))
            {
                item = new Item();
            }

            if (kind == 0)
            {
                Armor[slot] = item;
            }
            else if (kind == 1)
            {
                Vanity[slot] = item;
            }
            else if (kind == 2)
            {
                Dye[slot] = item;
            }
        }
    }

    internal static bool IsAllowedForSlot(Item item, int slot)
    {
        if (item is null || item.IsAir || slot < 0 || slot >= SlotCount)
        {
            return false;
        }

        return slot switch
        {
            0 => item.headSlot >= 0,
            1 => item.bodySlot >= 0,
            2 => item.legSlot >= 0,
            _ => false
        };
    }

    internal static bool IsDye(Item item) => item is not null && !item.IsAir && item.dye > 0;

    private static bool CanUseArmorItem(Item item) => (!item.expertOnly || Main.expertMode) && (!item.masterOnly || Main.masterMode);

    private static Item[] CreateItems() => [new(), new(), new()];

    private void EnsureArrays()
    {
        for (int i = 0; i < SlotCount; i++)
        {
            Armor[i] ??= new Item();
            Vanity[i] ??= new Item();
            Dye[i] ??= new Item();
        }
    }

    private void EnsureLoadouts()
    {
        for (int loadout = 0; loadout < 3; loadout++)
        {
            for (int slot = 0; slot < SlotCount; slot++)
            {
                loadoutArmor[loadout, slot] ??= new Item();
                loadoutVanity[loadout, slot] ??= new Item();
                loadoutDye[loadout, slot] ??= new Item();
            }
        }
    }

    private static List<TagCompound> SaveItems(Item[] items)
    {
        List<TagCompound> result = new(SlotCount);
        result.AddRange(items.Select(item => ItemIO.Save(item ?? new Item())));
        return result;
    }

    private static void LoadItems(TagCompound tag, string key, Item[] destination)
    {
        if (tag.ContainsKey(key))
        {
            var saved = tag.GetList<TagCompound>(key);
            for (int i = 0; i < Math.Min(SlotCount, saved.Count); i++)
            {
                destination[i] = ItemIO.Load(saved[i]);
            }
        }
    }

    private static void LoadItems(TagCompound tag, string key, int loadout, Item[,] destination)
    {
        if (tag.ContainsKey(key))
        {
            var saved = tag.GetList<TagCompound>(key);
            for (int i = 0; i < Math.Min(SlotCount, saved.Count); i++)
            {
                destination[loadout, i] = ItemIO.Load(saved[i]);
            }
        }
    }

    private static Item[] GetLoadoutItems(int loadout, Item[,] source)
    {
        var result = new Item[SlotCount];
        for (int slot = 0; slot < SlotCount; slot++)
        {
            result[slot] = source[loadout, slot];
        }

        return result;
    }

    private List<bool> SaveHidden(int loadout)
    {
        List<bool> result = new(SlotCount);
        for (int slot = 0; slot < SlotCount; slot++)
        {
            result.Add(loadoutHide[loadout, slot]);
        }

        return result;
    }

    private static void LoadHidden(TagCompound tag, string key, bool[] destination)
    {
        if (tag.ContainsKey(key))
        {
            var saved = tag.GetList<bool>(key);
            for (int i = 0; i < Math.Min(SlotCount, saved.Count); i++)
            {
                destination[i] = saved[i];
            }
        }
    }

    private static void LoadHidden(TagCompound tag, string key, int loadout, bool[,] destination)
    {
        if (tag.ContainsKey(key))
        {
            var saved = tag.GetList<bool>(key);
            for (int i = 0; i < Math.Min(SlotCount, saved.Count); i++)
            {
                destination[loadout, i] = saved[i];
            }
        }
    }
}

internal static class ExtraArmorUI
{
    private const int ArmorContext = ItemSlot.Context.EquipArmor;
    private const int VanityContext = ItemSlot.Context.EquipArmorVanity;
    private const int DyeContext = ItemSlot.Context.EquipDye;

    internal static void Draw()
    {
        if (Main.playerInventory && Main.EquipPage == 0 && !Main.LocalPlayer.dead)
        {
            ExtraArmorPlayer player = Main.LocalPlayer.GetModPlayer<ExtraArmorPlayer>();
            ExtraArmorPlayer.HoveredArmorType = 0;
            float originalScale = Main.inventoryScale;
            Main.inventoryScale = 0.85f;
            float scale = Main.inventoryScale;
            int y = AccessorySlotLoader.DrawVerticalAlignment;
            int vanillaArmorX = Main.screenWidth - 64 - 28;
            int functionalX = vanillaArmorX - 3 * 47;
            int vanityX = functionalX - 47;
            int dyeX = vanityX - 47;

            for (int slot = 0; slot < ExtraArmorPlayer.SlotCount; slot++)
            {
                int rowY = y + (int)(slot * 56f * scale);
                DrawSlot(player.Armor, ArmorContext, slot, functionalX, rowY, slot, false);
                DrawVanitySlot(player, slot, vanityX, rowY);
                DrawSlot(player.Dye, DyeContext, slot, dyeX, rowY, slot, true);
            }

            Main.inventoryScale = originalScale;
        }
    }

    private static void DrawVanitySlot(ExtraArmorPlayer extra, int slot, int x, int y)
    {
        int actualSlot = 10 + slot;
        Vector2 position = new(x, y);
        int slotSize = (int)(TextureAssets.InventoryBack.Width() * Main.inventoryScale);
        Rectangle bounds = new(x, y, slotSize, slotSize);
        Player localPlayer = Main.LocalPlayer;
        Item original = localPlayer.armor[actualSlot];
        localPlayer.armor[actualSlot] = extra.Vanity[slot];

        if (bounds.Contains(Main.mouseX, Main.mouseY) && !PlayerInput.IgnoreMouseInterface)
        {
            localPlayer.mouseInterface = true;
            Main.armorHide = true;
            bool allow = Main.mouseItem.IsAir || ExtraArmorPlayer.IsAllowedForSlot(Main.mouseItem, slot);
            ItemSlot.Handle(localPlayer.armor, VanityContext, actualSlot, allow);
            ItemSlot.MouseHover(localPlayer.armor, VanityContext, actualSlot);
        }

        ItemSlot.Draw(Main.spriteBatch, localPlayer.armor, VanityContext, actualSlot, position);

        if (extra.Vanity[slot].IsAir)
        {
            Texture2D placeholder = TextureAssets.Extra[ExtrasID.EquipIcons].Value;
            int frame = slot switch { 0 => 3, 1 => 9, 2 => 15, _ => 3 };
            Rectangle source = placeholder.Frame(3, 7, frame % 3, frame / 3);
            source.Width -= 2;
            source.Height -= 2;
            Main.spriteBatch.Draw(placeholder, position + TextureAssets.InventoryBack.Size() * Main.inventoryScale / 2f, source, Color.White * 0.05f, 0f, source.Size() / 2f, Main.inventoryScale, SpriteEffects.None, 0f);
        }

        extra.Vanity[slot] = localPlayer.armor[actualSlot];
        localPlayer.armor[actualSlot] = original;
    }

    private static void DrawSlot(Item[] items, int context, int slot, int x, int y, int armorSlot, bool dye)
    {
        Vector2 position = new(x, y);
        int slotSize = (int)(TextureAssets.InventoryBack.Width() * Main.inventoryScale);
        Rectangle bounds = new(x, y, slotSize, slotSize);
        if (bounds.Contains(Main.mouseX, Main.mouseY) && !PlayerInput.IgnoreMouseInterface)
        {
            Player localPlayer = Main.LocalPlayer;
            localPlayer.mouseInterface = true;
            Main.armorHide = true;
            bool allow = Main.mouseItem.IsAir || (dye ? ExtraArmorPlayer.IsDye(Main.mouseItem) : ExtraArmorPlayer.IsAllowedForSlot(Main.mouseItem, armorSlot));

            if (context == ArmorContext)
            {
                Item original = localPlayer.armor[slot];
                localPlayer.armor[slot] = items[slot];
                ItemSlot.Handle(localPlayer.armor, context, slot, allow);
                items[slot] = localPlayer.armor[slot];
                localPlayer.armor[slot] = original;
            }
            else
            {
                ItemSlot.Handle(items, context, slot, allow);
            }

            ItemSlot.MouseHover(items, context, slot);
            if (context == ArmorContext && !items[slot].IsAir && Main.HoverItem.type == items[slot].type)
            {
                ExtraArmorPlayer.HoveredArmorType = items[slot].type;
                Main.HoverItem.tooltipContext = 23;
                Main.HoverItem.tooltipSlot = slot;
                Main.HoverItem.wornArmor = true;
            }
        }

        ItemSlot.Draw(Main.spriteBatch, items, context, slot, position);
    }
}

public sealed class ExtraArmorTooltipGlobalItem : GlobalItem
{
    public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
    {
        if (ExtraArmorPlayer.HoveredArmorType > 0 && item.type == ExtraArmorPlayer.HoveredArmorType)
        {
            ExtraArmorPlayer extra = Main.LocalPlayer.GetModPlayer<ExtraArmorPlayer>();
            Item head = extra.Armor[0];
            Item body = extra.Armor[1];
            Item legs = extra.Armor[2];
            ArmorSetBonus.QueryContext context = new()
            {
                HeadItem = head.IsAir ? 0 : head.type,
                BodyItem = body.IsAir ? 0 : body.type,
                LegItem = legs.IsAir ? 0 : legs.type
            };

            var candidates = ArmorSetBonuses.SetsContaining[item.type];
            ArmorSetBonus best = null;
            ArmorSetBonus.QueryResult bestResult = default;
            foreach (ArmorSetBonus candidate in candidates)
            {
                ArmorSetBonus.QueryResult result = candidate.QueryCount(context);
                if (best == null || result.ItemsNeeded > bestResult.ItemsNeeded || (result.ItemsNeeded == bestResult.ItemsNeeded && result.ItemsFound > bestResult.ItemsFound))
                {
                    best = candidate;
                    bestResult = result;
                }
            }

            if (best != null)
            {
                foreach (var line in tooltips.Where(line => line.Mod == "Terraria" && line.Name == "SetBonus"))
                {
                    line.Text = best.GetTooltipForWornArmor(context, bestResult);
                    line.Color = bestResult.Complete ? Color.LimeGreen : new Color(130, 130, 130);
                    break;
                }
            }
        }
    }
}

public sealed class ExtraArmorUISystem : ModSystem
{
    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int index = layers.FindIndex(layer => layer.Name == "Vanilla: Inventory");
        if (index >= 0)
        {
            layers.Insert(index + 1, new LegacyGameInterfaceLayer(
                "EAS: Extra Armor",
                () =>
                {
                    if (ModContent.GetInstance<MyConfig>().Armor)
                    {
                        ExtraArmorUI.Draw();
                    }

                    return true;
                },
                InterfaceScaleType.UI));
        }
    }
}
