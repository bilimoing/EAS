using EAS.Content.Items.Accessories;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace EAS.Common.GlobalNPCs;

public class GlobalNPC_Loot : GlobalNPC
{
    public override bool InstancePerEntity => true;

    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.type == NPCID.SeekerHead)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<CurseStone>(), 50));
        }

        if (npc.type == NPCID.IchorSticker)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IchorStone>(), 50));
        }

        if (npc.type == NPCID.IceElemental)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FrozenStone>(), 150));
        }

        if (npc.type == NPCID.IcyMerman)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FrozenStone>(), 50));
        }
        
        if (npc.type is NPCID.BlackRecluse or NPCID.BlackRecluseWall)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VenomStone>(), 50));
        }
        
        if (npc.type is NPCID.JungleCreeper or NPCID.JungleCreeperWall)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VenomStone>(), 50));
        }
        
        if (npc.type is NPCID.DesertScorpionWalk or NPCID.DesertScorpionWall)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<VenomStone>(), 50));
        }
        
        if (npc.type == NPCID.SkeletronHead)
        {
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<DeathBone>()));
        }
    }
}