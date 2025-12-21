using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CreaturePillar : GolemPillar
{
   public Dictionary<Vector3, GameObject > StoneList = new Dictionary<Vector3, GameObject>();
   public GolemBossController Controller;
    public int Maxhealth;
    private void Start()
    {
        Controller = GetComponentInParent<GolemBossController>();
    }

    public override void ExecuteAbility()
    {
      int count = StoneList.Count;
        foreach (var item in StoneList)
        {
            Destroy(item.Value);
            //to do do animation
        }
        StoneList.Clear();
        count = count * 5;
        if(count > Maxhealth)
        {
            count = Maxhealth;
        }

        Controller.crystalHealth.heal(count, false);

    }
}
