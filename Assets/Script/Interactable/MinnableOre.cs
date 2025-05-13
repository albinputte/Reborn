using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinnableOre : MonoBehaviour, IInteractable
{
    public int oreCount = 3; 
    public GameObject orePrefab;
    [SerializeField] private ItemData ItemData;
    private Collider2D col;
    private SpriteRenderer mineRenderer; 
    [SerializeField] private float respawnTime; 
    [SerializeField] private int minOres; 
    [SerializeField] private int maxOres;
    [SerializeField] public InteractableType Type;
    [SerializeField] private GameObject Button;
    [SerializeField] private Material NewMaterial;
    [SerializeField] private Material OldMaterial;
    
    public InteractableType type { get => Type; set => Type = value; }
    public void Start()
    {
        mineRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        oreCount = Random.Range(minOres, maxOres);
    }

    public void Interact()
    {
        if (oreCount > 0)
        {
            
            GameObject ore = Instantiate(orePrefab, transform.position, Quaternion.identity);
            ore.GetComponent<WorldItem>().SetItem(ItemData, 1); 


            oreCount--;
            Debug.Log("Mined ore! Ores left: " + oreCount);

            
            Rigidbody2D rb = ore.AddComponent<Rigidbody2D>();
            if (rb != null)
                StartCoroutine(ApplyFloatDrop(rb));

          
            if (oreCount == 0)
                StartCoroutine(RespawnOres());
        }
        else
        {
            Debug.Log("The mine is empty.");
        }
    }
    public IEnumerator RespawnOres()
    {
        mineRenderer.enabled = false;
        col.enabled = false;
        yield return new WaitForSeconds(respawnTime);
        oreCount = Random.Range(minOres, maxOres);
        col.enabled = true;
        mineRenderer.enabled = true;
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

    public void NearPlayer()
    {
        mineRenderer.material = NewMaterial;
        Button.gameObject.SetActive(true);
    }

    public void LeavingPlayer()
    {
        mineRenderer.material = OldMaterial;
        Button.gameObject.SetActive(false);
    }
}


