using EAS.Common.Configs;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EAS.Common.AccessorySlot
{
    public class WingSlot : ModAccessorySlot
    {
        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context) => checkItem.wingSlot > 0;

        public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo) => item.wingSlot > 0;

        public override bool IsEnabled()
        {
            if (ModContent.GetInstance<MyConfig>().Wing)
            {
                return true;
            }
            return false;
        }

        public override bool IsVisibleWhenNotEnabled() => false;

        public override string FunctionalTexture => "EAS/Assets/Textures/UI/Wing";

        public override void OnMouseHover(AccessorySlotType context)
        {
            Main.hoverItemName = context switch
            {
                AccessorySlotType.FunctionalSlot or AccessorySlotType.VanitySlot => Language.GetTextValue("Mods.EAS.Message.16"),
                AccessorySlotType.DyeSlot => Language.GetTextValue("Mods.EAS.Message.4"),
                _ => Main.hoverItemName
            };
        }
    }
}

