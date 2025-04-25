

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
    }

    // Call sounds like this:
    //SoundManager.PlaySound(SoundType.SwordAttack);
}