using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyFountain : MonoBehaviour,IInteractable
{
    [SerializeField] public InteractableType Type;
    public InteractableType type { get => Type; set => Type = value; }
    [SerializeField] private ItemData NonFilledGrail;
    [SerializeField] private ItemData FilledGrail;
    [SerializeField] private GameObject GrailPrefab;
    [SerializeField] private Material NewMaterial;
    [SerializeField] private Material OldMaterial;
    [SerializeField] private SpriteRenderer FountainRenderer;


    public void Interact()
    {
        if (SearchForGrail(NonFilledGrail))
        {
            GameObject ore = Instantiate(GrailPrefab, transform.position, Quaternion.identity);
            ore.GetComponent<WorldItem>().SetItem(FilledGrail, 1);

            Rigidbody2D rb = ore.AddComponent<Rigidbody2D>();
            if (rb != null)
                StartCoroutine(ApplyFloatDrop(rb));
        }
    }

    public void LeavingPlayer()
    {
        FountainRenderer.material = NewMaterial;
    }

    public void NearPlayer()
    {
        FountainRenderer.material = OldMaterial;
     
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
    public bool SearchForGrail(ItemData NonFilledGrail)
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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
