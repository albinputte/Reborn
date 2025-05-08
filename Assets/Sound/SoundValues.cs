

using System;

namespace SmallHedge.SoundManager
{
    [Serializable]
    public enum SoundType
    {
        SwordAttack1,
        SwordAttack2,
        SwordAttack3,
        MonsterHitSound1,
        MineCoal,
        MineIron,
        ItemDrop,
        InteractBerryBush,
        PlayerTakeDamage,
        PickUpSound,
        CampfireCrafting,
        AnvilCrafting,
        CraftingBenchCrafting,
        StoneGhost_Looking,
        StoneGhost_Rising,
        StoneGhost_StartAttack,
        StoneGhost_Hit,

    }

    // Call sounds like this:
    //SoundManager.PlaySound(SoundType.SwordAttack);
}