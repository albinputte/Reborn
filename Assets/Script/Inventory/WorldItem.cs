using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItem : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private InventorySO inventory;
    [SerializeField] private ItemData item;
    [SerializeField] private Sprite sprite; 
    private int quantity;

    [SerializeField] private bool IsADrop;


    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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




}
