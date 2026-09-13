using EAS.Common.Configs;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EAS.Common.AccessorySlot;

public class GloveSlot : ModAccessorySlot
{
    public override bool CanAcceptItem(Item checkItem, AccessorySlotType context) => checkItem.handOnSlot > 0;
    public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo) => item.handOnSlot > 0;
    public override bool IsEnabled()
    {
        if (ModContent.GetInstance<MyConfig>().Glove)
        {
            return true;
        }
        return false;
    }

    public override bool IsVisibleWhenNotEnabled() => false;

    public override string FunctionalTexture => "EAS/Assets/Textures/UI/Glove";

    public override void OnMouseHover(AccessorySlotType context)
    {
        Main.hoverItemName = context switch
        {
            AccessorySlotType.FunctionalSlot or AccessorySlotType.VanitySlot => Language.GetTextValue("Mods.EAS.Message.17"),
            AccessorySlotType.DyeSlot => Language.GetTextValue("Mods.EAS.Message.4"),
            _ => Main.hoverItemName
        };
    }
}
