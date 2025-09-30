using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetUiRefrences : MonoBehaviour
{
    public static GetUiRefrences instance;
    public GameObject[] InputMangerUiObjectRefrences;
    void Awake()
    {
        instance = this;
    }

}
