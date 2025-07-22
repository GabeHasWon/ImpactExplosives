using Terraria.GameContent.ItemDropRules;

namespace ImpactExplosives.Items;

internal class RedGel : ModItem
{
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.Gel);
        Item.color = Color.Transparent;
        Item.alpha = 90;
    }
}

internal class RedGelReplacement : GlobalNPC
{
    public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
    {
        if (npc.netID == NPCID.RedSlime)
        {
            var l = npcLoot.Get();

            foreach (var rule in l)
            {
                if (rule is CommonDrop common && common.itemId == ItemID.Gel)
                {
                    common.itemId = ModContent.ItemType<RedGel>();
                    common.amountDroppedMaximum = 1;
                    common.amountDroppedMinimum = 1;
                }
            }
        }
    }
}
