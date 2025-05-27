using UnityEngine;

public class ConquestEnemy : MonoBehaviour
{
    private ConquestPressureplate originPlate;
    private bool CanNotify = true;
    public void Init(ConquestPressureplate plate)
    {
        originPlate = plate;
    }

    public void Die()
    {
        if (CanNotify)
        {
            CanNotify = false;
            originPlate?.NotifyEnemyDied(gameObject);
        }
    }
}
