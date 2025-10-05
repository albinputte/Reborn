using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GetTileMapRefrence : MonoBehaviour
{
    public static GetTileMapRefrence Instance;
    public Tilemap Ground;
    void Awake()
    {
        Instance = this;
    }

}
