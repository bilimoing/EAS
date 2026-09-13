using EAS.Common.Configs;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EAS.Common.AccessorySlot
{
    public class BalloonSlot : ModAccessorySlot
    {
        public override bool CanAcceptItem(Item checkItem, AccessorySlotType context) => checkItem.balloonSlot > 0;
        public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo) => item.balloonSlot > 0;
        public override bool IsEnabled()
        {
            if (ModContent.GetInstance<MyConfig>().Balloon)
            {
                return true;
            }
            return false;
        }


        public override bool IsVisibleWhenNotEnabled() => false;
        public override string FunctionalTexture => "Terraria/Images/Item_" + 1164;

        public override void OnMouseHover(AccessorySlotType context)
        {

            switch (context)
            {
                case AccessorySlotType.FunctionalSlot:
                case AccessorySlotType.VanitySlot:
                    Main.hoverItemName = Language.GetTextValue("Mods.EAS.Message.5");
                    break;
                case AccessorySlotType.DyeSlot:
                    Main.hoverItemName = Language.GetTextValue("Mods.EAS.Message.4");
                    break;
            }
        }
    }
}
