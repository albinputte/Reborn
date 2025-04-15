using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class StatSystem : MonoBehaviour
{
   public List<Stats> Stats = new List<Stats>();
   public Dictionary<StatsType, Stats> StatList = new Dictionary<StatsType, Stats>();
    public static StatSystem instance;  
    public void Awake()
    {
        StatList = Stats.ToDictionary(s => s.type, s => s);
        instance = this;
    }

    public float GetStat(StatsType type) { return StatList[type].ModfierValue; }

    public void AddStats(StatsType type, float amount) => StatList[type].AddModfier(amount); 
    public void RemoveStats(StatsType type, float amount) => StatList[type].RemoveModfier(amount); 

    public void ResetStats(StatsType type) => StatList[type].ResetValue();




}
