using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CreaturePillar : GolemPillar
{
   public Dictionary<Vector3, GameObject > StoneList = new Dictionary<Vector3, GameObject>();
   public GolemBossController Controller;
    public float FirstPhase, LastPhase, timeToDestroy;
    public int Maxhealth;
    private void Start()
    {
        Controller = GetComponentInParent<GolemBossController>();
    }

    public override void ExecuteAbility()
    {
       StartCoroutine(StartHeal());

    }

    public IEnumerator StartHeal()
    {
        int count = StoneList.Count;
        foreach (var item in StoneList) {
            BigCrystalFalling bigCrystal = item.Value.gameObject.GetComponent<BigCrystalFalling>();
            bigCrystal.StartRoots();
        }
        yield return new WaitForSeconds(FirstPhase);
        foreach (var item in StoneList)
        {
            BigCrystalFalling bigCrystal = item.Value.gameObject.GetComponent<BigCrystalFalling>();
            bigCrystal.StartDestroy();
        }
        yield return new WaitForSeconds(LastPhase);
        count = count * 5;
        if (count > Maxhealth)
        {
            count = Maxhealth;
        }
        Controller.crystalHealth.heal(count, false);
        yield return new WaitForSeconds(timeToDestroy);
        foreach (var item in StoneList)
        {
            Destroy(item.Value);
            //to do do animation
        }
        StoneList.Clear();
       

        
    }
}
