using System;
using UnityEngine;
using UnityEngine.Playables;

public class EndManager : MonoBehaviour
{
    public PlayableDirector timelineDirector;
    public event Action<bool> StoneManOffering;
    public static EndManager instance;

    public static bool HasDropped = false;
    public static int Killcount = 0;
    private int OfferingCount;

    public void Awake()
    {
        StoneManOffering += Offering;
        instance = this;
 
    }
    public void TriggerEndEvent()
    {
        if (timelineDirector != null)
        {
          

            timelineDirector.Play();
            Debug.Log("Timeline Started");
        }
        else
        {
            Debug.LogWarning("PlayableDirector is not assigned.");
        }
    }


    public void Offering(bool WasOffered)
    {
        if (!WasOffered)
        {
            OfferingCount--;
        }
        else
        {
            OfferingCount++;
        }

        if(OfferingCount >= 2)
        {
            TriggerEndEvent();
        }
    }
}
