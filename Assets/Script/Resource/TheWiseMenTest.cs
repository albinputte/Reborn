using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWiseMenTest : ResourceNodeBase
{
    [SerializeField] private ItemData requiredItem;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private GameObject itemHeldObject;
    [SerializeField] private bool hasItem;
    [SerializeField] private bool showHintWhenEmpty = true;

    private bool hasInteractedThisCycle;
    private bool cooldownRunning;

    public override void Interact()
    {
        if (hasInteractedThisCycle)
        {
            hasInteractedThisCycle = false;
            return;
        }

        hasInteractedThisCycle = true;

        if (RemoveItemFromInventory(requiredItem) && !hasItem)
        {
            SoundManager.PlaySound(SoundType.SwapItem_Inventory);
            hasItem = true;
            itemHeldObject.SetActive(true);
            EndManager.instance.Offering(hasItem);
            interactHint?.SetActive(false);
            StartCoroutine(Cooldown());
        }
        else if (hasItem)
        {
            hasItem = false;
            itemHeldObject.SetActive(false);
            EndManager.instance.Offering(hasItem);

            GameObject dropped = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            dropped.GetComponent<WorldItem>().SetItem(requiredItem, 1);

            Rigidbody2D rb = dropped.AddComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            StartCoroutine(ApplyFloatDrop(rb));

            StartCoroutine(Cooldown());
        }

        if (!cooldownRunning)
            playerInput.isInteracting = true;
    }

    private IEnumerator Cooldown()
    {
        cooldownRunning = true;
        yield return new WaitForSeconds(0.3f);
        playerInput.isInteracting = true;
        cooldownRunning = false;
    }

    public override void NearPlayer()
    {
        base.NearPlayer();
        if (!hasItem && showHintWhenEmpty && interactHint != null)
            interactHint.SetActive(true);
    }

    public override void LeavingPlayer()
    {
        base.LeavingPlayer();
        if (!hasItem && interactHint != null)
            interactHint.SetActive(false);
    }
}