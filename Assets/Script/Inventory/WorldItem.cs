using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private InventorySO inventory;
    [SerializeField] private ItemData item;
    [SerializeField] private Sprite sprite;
    [SerializeField] private int quantity;

    [SerializeField] private bool IsADrop;

    [SerializeField] private Collider2D col;
    private Transform ItemTrans;

    public float hoverAmount;
    public float hoverCycleTime;
    public float hoverDelay;
    public float hoverStepamount;



    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        ItemTrans = GetComponent<Transform>();  
    }
    public void Start()
    {
        if (!IsADrop && item != null)
            SetItem(item, quantity);
    }

    public void SetItem(ItemData item, int Quantity)
    {
        this.item = item;
        sprite = item.Icon;
        spriteRenderer.sprite = item.Icon;
        this.quantity = Quantity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Player"))
            {
                inventory.AddItem(item, quantity, null);
                Destroy(gameObject);
            }
        }

    }

    public void EnableCollider()
    {
        col.enabled = true;


    }

    public void StartHover() => StartCoroutine(HoverEffect());

    public IEnumerator HoverEffect()
    {
        var HoverStep = hoverAmount / hoverStepamount;
        var HoverCycle = hoverCycleTime / hoverStepamount;
        
        for (int i = 0; i < hoverStepamount; i++)
        {
            transform.position += new Vector3(0, HoverStep);
            yield return new WaitForSeconds(HoverCycle);
        }
        yield return new WaitForSeconds(hoverDelay);
        for (int i = 0;i < hoverStepamount * 2; i++)
        {
            transform.position -= new Vector3(0, HoverStep);
            yield return new WaitForSeconds(HoverCycle);
        }
        yield return new WaitForSeconds(hoverDelay);
        for (int i = 0; i < hoverStepamount; i++)
        {
            transform.position += new Vector3(0, HoverStep);
            yield return new WaitForSeconds(HoverCycle);
        }
        StartCoroutine(HoverEffect());
    }


}
