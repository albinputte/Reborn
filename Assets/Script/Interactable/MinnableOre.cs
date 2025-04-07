using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinnableOre : MonoBehaviour
{
    public Sprite[] VeinSprite;
    public int OreAmount;
    public int Hardeness;
    public int pickaxePower;
    public float SwingSpeed;
    public float ChanceToCritical;
    public float SwingColdown;
    void Start()
    {
        if (pickaxePower > Hardeness)
        {
            ChanceToCritical = Mathf.Clamp01((float)(pickaxePower - Hardeness) / Hardeness);
        }
        else
        {
            ChanceToCritical = 0;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool TryMine()
    {
        if (Random.value <= ChanceToCritical)  // Random.value returns a float between 0 and 1
        {
            OreAmount--; // Reduce ore amount when mined successfully
            if (OreAmount <= 0)
            {
                Destroy(gameObject); // Destroy the ore when depleted
            }
            return true; // Mining was successful
        }
        return false; // Mining failed
    }

}


