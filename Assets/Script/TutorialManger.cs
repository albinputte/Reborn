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

    public TutorialState currentState = TutorialState.Start;

    private void Start()
    {
        instance = this;
        TutorialIsActive = true;
        EnterState(currentState);
    }

    public void EnterState(TutorialState state)
    {
        currentState = state;

        switch (state)
        {
            case TutorialState.PickedUpSword:
               StartCoroutine(OpenInventoryTimer());
                break;

            case TutorialState.OpenInventory:
           TutorialEnd();
                break;

          

           

            
        }
    }

    public IEnumerator OpenInventoryTimer()
    {
        yield return new WaitForSeconds(8f);
        if (currentState == TutorialState.PickedUpSword)
            TutorialUiManger.Instance.ShowMessage(Messages[0]);

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