using EAS.Common.Configs;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EAS.Common.AccessorySlot
{
    public class MusicBoxSlot : ModAccessorySlot
    {
        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context) => checkItem.Name.Contains(Language.GetTextValue("Mods.EAS.Message.11"));
        public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo) => CanAcceptItem(item, (AccessorySlotType)10);
        public override bool IsEnabled()
        {
            if (ModContent.GetInstance<MyConfig>().MusicBox)
            {
                return true;
            }
            return false;
        }

        public override bool IsVisibleWhenNotEnabled() => false;

        public override string FunctionalTexture => "Terraria/Images/Item_" + ItemID.MusicBox;

        public override void OnMouseHover(AccessorySlotType context)
        {
            Main.hoverItemName = context switch
            {
                AccessorySlotType.FunctionalSlot or AccessorySlotType.VanitySlot => Language.GetTextValue("Mods.EAS.Message.11"),
                AccessorySlotType.DyeSlot => Language.GetTextValue("Mods.EAS.Message.4"),
                _ => Main.hoverItemName
            };
        }
    }
}
