using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Stats 
{
    public StatsType type;
    public float baseValue;
    public float ModfierValue;

    private List<float> Modfiers = new List<float>();

    public float totalValue => baseValue + Modfiers.Sum();

    public void AddModfier(float amount) {  Modfiers.Add(amount); ModfierValue = totalValue; }
    public void RemoveModfier(float amount) { Modfiers.Remove(amount); ModfierValue = totalValue; }
    public void ResetValue() { Modfiers.Clear(); ModfierValue = baseValue; }

}
