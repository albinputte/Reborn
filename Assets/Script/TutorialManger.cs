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
           

            case TutorialState.EnemyDefeated:
                TutorialUiManger.Instance.ShowMessage(Messages[4]);

                if (chestPrefab && chestSpawnPos)
                {
                    Instantiate(chestPrefab, chestSpawnPos.position, Quaternion.identity);
                }
                break;

            case TutorialState.RewardCollected:
                TutorialUiManger.Instance.ShowMessage(Messages[5]);
                StartCoroutine(TutorialEnd());
                break;

            case TutorialState.Complete:
                // Could fade out or show "Tutorial Complete!"
                break;
        }
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

    private IEnumerator TutorialEnd()
    {

        yield return new WaitForSeconds(8f);
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