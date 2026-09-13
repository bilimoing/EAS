using EAS.Common.Configs;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EAS.Common.AccessorySlot
{
    public class Expertslot : ModAccessorySlot
    {
        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
        {
            return checkItem.expert;
        }

        public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo)
        {
            return item.expert;
        }

        public override bool IsEnabled()
        {
            if (ModContent.GetInstance<MyConfig>().Expert)
            {
                return true;
            }
            return false;
        }

        public override bool IsVisibleWhenNotEnabled() => false;

        public override string FunctionalTexture => "EAS/Assets/Textures/UI/Expert";

        public override void OnMouseHover(AccessorySlotType context)
        {
            Main.hoverItemName = context switch
            {
                AccessorySlotType.FunctionalSlot or AccessorySlotType.VanitySlot => Language.GetTextValue("Mods.EAS.Message.7"),
                AccessorySlotType.DyeSlot => Language.GetTextValue("Mods.EAS.Message.4"),
                _ => Main.hoverItemName
            };
        }
    }
}
