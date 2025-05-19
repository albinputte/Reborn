using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class StatSystem : MonoBehaviour
{
    public List<Stats> Stats = new List<Stats>();
    [SerializeField]
    public Dictionary<StatsType, Stats> StatList = new Dictionary<StatsType, Stats>();
    public static StatSystem instance;
    private void Awake()
    {
        instance = this;

        // Fill in missing StatsTypes with default Stats
        foreach (StatsType type in System.Enum.GetValues(typeof(StatsType)))
        {
            if (!Stats.Any(s => s.type == type))
            {
                Stats newStat = new Stats { type = type };
                newStat.baseValue = 0;
                Stats.Add(newStat);
            }
        }

        StatList = Stats.ToDictionary(s => s.type, s => s);
    }

    public float GetStat(StatsType type) { return StatList[type].ModfierValue; }

    public void AddStats(StatsType type, float amount) => StatList[type].AddModfier(amount); 
    public void RemoveStats(StatsType type, float amount) => StatList[type].RemoveModfier(amount); 

    public void ResetStats(StatsType type) => StatList[type].ResetValue();




}
