using System.Collections;
using UnityEngine;

public abstract class ResourceNodeBase : MonoBehaviour, IInteractable
{
    [Header("Interaction Base Settings")]
    [SerializeField] protected Material highlightMaterial;
    [SerializeField] protected Material defaultMaterial;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected GameObject interactHint;
    [SerializeField] public InteractableType Type;

    public InteractableType type { get => Type; set => Type = value; }

    protected PlayerInputManger playerInput;

    protected virtual void Awake()
    {
        playerInput = FindAnyObjectByType<PlayerInputManger>();
    }

    public abstract void Interact();

    public virtual void NearPlayer()
    {
        if (spriteRenderer != null)
            spriteRenderer.material = highlightMaterial;

        if (interactHint != null)
            interactHint.SetActive(true);
    }

    public virtual void LeavingPlayer()
    {
        if (spriteRenderer != null)
            spriteRenderer.material = defaultMaterial;

        if (interactHint != null)
            interactHint.SetActive(false);
    }

    protected bool RemoveItemFromInventory(ItemData itemToRemove)
    {
        var inventory = InventoryController.Instance.inventoryData;
        for (int i = 0; i < inventory.Inventory.Count; i++)
        {
            InventoryItem item = inventory.GetSpecificItem(i);
            if (item.item == itemToRemove)
            {
                inventory.RemoveItem(i, 1);
                return true;
            }
        }
        return false;
    }

    protected IEnumerator ApplyFloatDrop(Rigidbody2D rb)
    {
        if (rb == null) yield break;

        Vector2 force = new Vector2(Random.Range(-1f, 1f), 1.5f).normalized * 4f;
        rb.AddForce(force, ForceMode2D.Impulse);

        yield return new WaitForSeconds(0.2f);
        if (rb == null) yield break;
        rb.gravityScale = 1f;

        yield return new WaitForSeconds(0.7f);
        if (rb == null) yield break;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
    }
}
