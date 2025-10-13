using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.Playables;

public class EndManager : MonoBehaviour
{
    public PlayableDirector timelineDirector;
    public event Action<bool> StoneManOffering;
    public static EndManager instance;
    public CinemachineBrain cinemachineBrain;

    public static bool HasDropped = false;
    public static int Killcount = 0;
    private int OfferingCount;

    public void Awake()
    {
        StoneManOffering += Offering;
        instance = this;
        HasDropped = false;
         Killcount = 0;
       

}

    public void Start()
    {
        SceneManger.instance.OnAllEssentialScenesLoaded += PrepareRef;
     
    }
    private void OnDisable()
    {
        StoneManOffering -= Offering;
    }

    public void PrepareRef()
    {
        timelineDirector = FindAnyObjectByType<PlayableDirector>();
        cinemachineBrain = FindAnyObjectByType<CinemachineBrain>();
        if (timelineDirector == null || cinemachineBrain == null) return;

        // Iterate over all track bindings and set them dynamically
        foreach (var output in timelineDirector.playableAsset.outputs)
        {
            if (output.streamName == "Cinemachine Track") // Match your track name
            {
                timelineDirector.SetGenericBinding(output.sourceObject, cinemachineBrain);
            }
        }
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
