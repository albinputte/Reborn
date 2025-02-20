using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Data/HealthData", order = 1)]
public class HealthData : ScriptableObject
{
    public int health;
    public int maxHealth;
    public bool hasInvincibilty;
    public float invincibiltyTime;
    

}
