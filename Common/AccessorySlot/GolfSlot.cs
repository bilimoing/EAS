using EAS.Common.Configs;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace EAS.Common.AccessorySlot;

public class GolfSlot : ModAccessorySlot
{
    public override bool CanAcceptItem(Item checkItem, AccessorySlotType context) => checkItem.Name.Contains(Language.GetTextValue("Mods.EAS.Message.19"));
    public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo) => CanAcceptItem(item, (AccessorySlotType)10);
    public override bool IsEnabled()
    {
        if (ModContent.GetInstance<MyConfig>().Golf)
        {
            return true;
        }
        return false;
    }

    public override bool IsVisibleWhenNotEnabled() => false;

    public override string FunctionalTexture => "EAS/Assets/Textures/UI/Golf";

    public override void OnMouseHover(AccessorySlotType context)
    {
        Main.hoverItemName = context switch
        {
            AccessorySlotType.FunctionalSlot or AccessorySlotType.VanitySlot => Language.GetTextValue("Mods.EAS.Message.18"),
            AccessorySlotType.DyeSlot => Language.GetTextValue("Mods.EAS.Message.4"),
            _ => Main.hoverItemName
        };
    }
}
