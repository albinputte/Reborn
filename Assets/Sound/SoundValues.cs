

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
        CutGrass,
        Conquest_Finish_Ding_1,
        Conquest_Finish_Ding_2,
        Conquest_Finish_Ding_3,
        Conquest_Finish_Ding_4,
        Torch_LitUp,
        Conquest_Chest_Landing,


    }

    // Call sounds like this:
    //SoundManager.PlaySound(SoundType.SwordAttack);
}