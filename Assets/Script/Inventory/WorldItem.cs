using SmallHedge.SoundManager;
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
    private bool IsPickingUp;

    [SerializeField] private bool IsADrop;
    [SerializeField] private Collider2D col;
    private Transform ItemTrans;

    [Header("Hover Settings")]
    public float hoverAmount;
    public float hoverCycleTime;
    public float hoverDelay;
    public float hoverStepamount;

    [Header("Magnet Settings")]
  
    public float pickupRadius = 3f;
    public float moveSpeed = 5f;
    private Transform player;
    private bool CanBeSucked = false;

    public void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        ItemTrans = GetComponent<Transform>();
        IsPickingUp = false;
    }

    public void Start()
    {
        if (!IsADrop && item != null)
            SetItem(item, quantity);

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    public void SetItem(ItemData item, int Quantity)
    {
        this.item = item;
        sprite = item.Icon;
        spriteRenderer.sprite = item.Icon;
        this.quantity = Quantity;
    }

    private void Update()
    {
        if (player == null || IsPickingUp) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance < pickupRadius && CanBeSucked)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Optional: Auto-pickup when very close
            if (distance < 0.2f)
                TriggerPickup();
        }
    }

    private void TriggerPickup()
    {
        if (IsPickingUp) return;
        IsPickingUp = true;
        inventory.AddItem(item, quantity, null);
        SoundManager.PlaySound(SoundType.PickUpSound);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && collision.CompareTag("Player"))
        {
            TriggerPickup(); // fallback in case collider is used
        }
    }

    public void EnableCollider()
    {
        col.enabled = true;
    }

    public void StartHover() => StartCoroutine(HoverEffect());

    public IEnumerator HoverEffect()
    {
        CanBeSucked = true;
        var HoverStep = hoverAmount / hoverStepamount;
        var HoverCycle = hoverCycleTime / hoverStepamount;

        for (int i = 0; i < hoverStepamount; i++)
        {
            transform.position += new Vector3(0, HoverStep);
            yield return new WaitForSeconds(HoverCycle);
        }
        yield return new WaitForSeconds(hoverDelay);
        for (int i = 0; i < hoverStepamount * 2; i++)
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
