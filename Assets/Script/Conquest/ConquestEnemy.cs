using UnityEngine;

public class ConquestEnemy : MonoBehaviour
{
    private ConquestPressureplate originPlate;

    public void Init(ConquestPressureplate plate)
    {
        originPlate = plate;
    }

    public void Die()
    {
        // Your enemy's death logic...
        originPlate?.NotifyEnemyDied(gameObject);

    }
}
