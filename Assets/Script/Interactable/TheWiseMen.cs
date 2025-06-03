using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWiseMen : MonoBehaviour, IInteractable
{
  

    [SerializeField] public InteractableType Type;
    public InteractableType type { get => Type; set => Type = value; }
    [SerializeField] private ItemData ItemNeededGrail;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private Material NewMaterial;
    [SerializeField] private Material OldMaterial;
    [SerializeField] private SpriteRenderer statueRenderer;
    public bool HasItem;


    public void Interact()
    {
        if (SearchForReqItem(ItemNeededGrail) && !HasItem)
        {
           HasItem = true;
        }
        else if (HasItem) {
            HasItem = false;
            GameObject ore = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            ore.GetComponent<WorldItem>().SetItem(ItemNeededGrail, 1);

            Rigidbody2D rb = ore.AddComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (rb != null)
                StartCoroutine(ApplyFloatDrop(rb));
        }
    }

    public void LeavingPlayer()
    {
        statueRenderer.material = NewMaterial;
    }

    public void NearPlayer()
    {
        statueRenderer.material = OldMaterial;

    }
    private IEnumerator ApplyFloatDrop(Rigidbody2D rb)
    {

        Vector2 force = new Vector2(Random.Range(-1f, 1f), 1.5f).normalized * 4f;
        if (rb == null)
            yield break;
        rb.AddForce(force, ForceMode2D.Impulse);


        yield return new WaitForSeconds(0.2f);
        if (rb == null)
            yield break;
        rb.gravityScale = 1f;


        yield return new WaitForSeconds(0.7f);
        if (rb == null)
            yield break;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
    }
    public bool SearchForReqItem(ItemData NonFilledGrail)
    {
        for (int i = 0; i < InventoryController.Instance.inventoryData.Inventory.Count; i++)
        {
            InventoryItem ItemToTest = InventoryController.Instance.inventoryData.GetSpecificItem(i);
            if (ItemToTest.item == NonFilledGrail)
            {
                InventoryController.Instance.inventoryData.RemoveItem(i, 1);
                return true;
            }

        }
        return false;
    }
}
