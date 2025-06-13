using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManger : MonoBehaviour
{
    public static TutorialManger instance;

    [Header("Tutorial Content")]
    public string[] Messages;
    public bool TutorialIsActive;

    [Header("Prefabs & Positions")]
    public GameObject EnemyPrefab;
    public Transform EnemySpawnPos;
    public GameObject chestPrefab;
    public Transform chestSpawnPos;
    private bool EisInteractedEnough;
    private int InteractionTimes;
    private string Lastinteractedname;
    private bool IsStoneInteractedWith;

    public TutorialState currentState = TutorialState.Start;
    public void Awake()
    {
        Time.timeScale = 1.0f;
    }
    private void Start()
    {
        instance = this;
        TutorialIsActive = true;
        EisInteractedEnough = true;
        IsStoneInteractedWith = false;
        EnterState(currentState);

    }

    public void EnterState(TutorialState state)
    {
        currentState = state;

        switch (state)
        {
            case TutorialState.PickedUpSword:
              OpenInventoryTimer();
                break;

            case TutorialState.OpenInventory:
           TutorialEnd();
                break;

          

           

            
        }
    }
    public bool EshouldAppear()
    {
        return EisInteractedEnough;
    }
    public void EwasInteractedWith(string ObjectName)
    {
        if (ObjectName != Lastinteractedname) {
            InteractionTimes++;
            Lastinteractedname = ObjectName;
        }
     
        if(InteractionTimes >= 2)
            EisInteractedEnough = false;
    }
    public void OpenInventoryTimer()
    {
        
        if (currentState == TutorialState.PickedUpSword)
            TutorialUiManger.Instance.ShowMessage(Messages[0]);

    }
    public bool PickaxeShouldAppear()
    {
        return IsStoneInteractedWith;
    }

    public void StoneMined()
    {
        IsStoneInteractedWith = true;   
    }
    // External event triggers:
    public void OnSwordPickedUp()
    {
        currentState = TutorialState.PickedUpSword;
        EnterState(TutorialState.PickedUpSword);
    }

    public void OnInventoryOpened()
    {
      
        currentState = TutorialState.OpenInventory;
        EnterState(TutorialState.OpenInventory);
    }

    public void OnSwordEquipped()
    {
        currentState = TutorialState.EquipSword;
        EnterState(TutorialState.EquipSword);
    }

    public void OnEnemyDefeated()
    {
        currentState = TutorialState.EnemyDefeated;
        EnterState(TutorialState.EnemyDefeated);
    }

    public void OnRewardCollected()
    {
        currentState = TutorialState.RewardCollected;
        EnterState(TutorialState.RewardCollected);
    }

    private void TutorialEnd()
    {

        
        TutorialUiManger.Instance.ClearMessage();
        TutorialIsActive = false;
        currentState = TutorialState.Complete;
        EnterState(TutorialState.Complete);
    }
}
public enum TutorialState
{
    Start,
    PickedUpSword,
    OpenInventory,
    EquipSword,
    EnterCombat,
    EnemyDefeated,
    RewardCollected,
    Complete
}