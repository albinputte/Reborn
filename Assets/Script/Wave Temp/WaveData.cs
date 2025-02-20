using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveData
{
    public int enemy1Count;
    public int enemy2Count;
    public int enemy3Count;
    public int enemy4Count;
    public float SpawnInterval;
    // Add more enemy types as needed

    public WaveData(int enemy1Count, int enemy2Count, int enemy3Count, int enemy4count, int SpawnInterval)
    {
        this.enemy1Count = enemy1Count;
        this.enemy2Count = enemy2Count;
        this.enemy3Count = enemy3Count;
        this.enemy4Count = enemy4count;
        this.SpawnInterval = SpawnInterval;
    }
}
