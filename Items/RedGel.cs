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
            var l = npcLoot.Get(false);

            foreach (var rule in l)
            {
                if (rule is CommonDrop common && common.itemId == ItemID.Gel)
                {
                    npcLoot.Remove(common);
                    npcLoot.Add(new CommonDrop(ModContent.ItemType<RedGel>(), 1));
                }
            }
        }
    }
}
