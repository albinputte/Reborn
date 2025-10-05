using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GetUiRefrences : MonoBehaviour
{
    public static GetUiRefrences instance;
    public GameObject[] InputMangerUiObjectRefrences;
    
    void Awake()
    {
        instance = this;
    }

}
