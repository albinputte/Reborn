using SmallHedge.SoundManager;
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
    [SerializeField] private GameObject Hint;
    public GameObject ItemHeld;
    public bool HasItem;
    private bool Timer;
    private PlayerInputManger playerInputManger;

    public void Start()
    {
        playerInputManger = FindAnyObjectByType<PlayerInputManger>();
    }
    private bool hasInteractedThisCycle = false;

    public void Interact()
    {
        // If this is the second call, reset and return immediately
        if (hasInteractedThisCycle)
        {
            hasInteractedThisCycle = false;
            return;
        }

        // First call – run interaction and set the flag
        hasInteractedThisCycle = true;

        if (SearchForReqItem(ItemNeededGrail) && !HasItem)
        {
            SoundManager.PlaySound(SoundType.SwapItem_Inventory);
            HasItem = true;
            StartCoroutine(Cooldown());
            ItemHeld.SetActive(true);
            EndManager.instance.Offering(HasItem);
            Hint.SetActive(false);
        }
        else if (HasItem)
        {
            HasItem = false;
            StartCoroutine(Cooldown());
            EndManager.instance.Offering(HasItem);
            GameObject ore = Instantiate(itemPrefab, transform.position, Quaternion.identity);
            ore.GetComponent<WorldItem>().SetItem(ItemNeededGrail, 1);
            ItemHeld.SetActive(false);

            Rigidbody2D rb = ore.AddComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (rb != null)
                StartCoroutine(ApplyFloatDrop(rb));
        }

        if (!Timer)
        {
            playerInputManger.isInteracting = true;
        }
    }

    public IEnumerator Cooldown()
    {
        Timer = true;
        yield return new WaitForSeconds(0.3f);
        playerInputManger.isInteracting = true;
        Timer = false;
       
    }


    public void LeavingPlayer()
    {
        statueRenderer.material = NewMaterial;
        if (!HasItem) { 
            Hint.SetActive(false);
        }
    }

    public void NearPlayer()
    {
        statueRenderer.material = OldMaterial;
        if (!HasItem)
        {
            Hint.SetActive(true);
        }
    }
    private IEnumerator ApplyFloatDrop(Rigidbody2D rb)
    {
        if (rb == null) yield break;
        Vector2[] directions =
        {
            Vector2.left,
            Vector2.right

        };
        float[] randomValues = { 0.4f, 0.4f };
        float checkDist = 0.6f;
        Collider2D[] ItemCol;
        if (Physics2D.Raycast(rb.position, Vector2.up, checkDist, LayerMask.GetMask("Cliff")))
        {
            ItemCol = rb.gameObject.GetComponents<Collider2D>();
            foreach (var col in ItemCol)
            {
                if (!col.isTrigger)
                {
                    col.enabled = false;
                }
            }


        }
        int i = 0;
        foreach (var dir in directions)
        {
            if (!Physics2D.Raycast(rb.position, dir, checkDist, LayerMask.GetMask("Cliff")))
            {
                randomValues[i] = dir.x;
            }
            i++;
        }

        Vector2 force = new Vector2(Random.Range(0, 0), 1.5f).normalized * 4f;
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
