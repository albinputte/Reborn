using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class SlotIndex : MonoBehaviour
{

    public int slotNumber;
    // Start is called before the first frame update
    void Start()
    {
        slotNumber = ExtractNumberFromString(gameObject.name);
    }

    // Update is called once per frame
    static int ExtractNumberFromString(string s)
    {
        // Remove all non-digit characters
        string cleanedString = Regex.Replace(s, @"\D", "");
        // Convert the cleaned string to an integer
        int number = string.IsNullOrEmpty(cleanedString) ? 0 : int.Parse(cleanedString);
        return number;
    }
}
