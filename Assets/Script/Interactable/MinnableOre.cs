using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinnableOre : MonoBehaviour, IInteractable
{
    public int oreCount = 3;
    private int HitCount;
    public int MaxHitCount;
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
    [SerializeField] private GameObject StoneParticles;
    [SerializeField] private bool HasShadow;
    [SerializeField] private GameObject Shadow;


    public InteractableType type { get => Type; set => Type = value; }
    public void Start()
    {
        mineRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        oreCount = Random.Range(minOres, maxOres);
        HitCount = MaxHitCount;
    }

    public void Interact()
    {
        if (oreCount > 0)
        {

            if (type == InteractableType.Minning)
            {
                SoundManager.PlaySound(SoundType.PickAxe_Sound);
                GameObject Particle = Instantiate(StoneParticles, transform.position, Quaternion.identity);
                Mine();
                if (HitCount > 0)
                    return;
            }
             

            GameObject ore = Instantiate(orePrefab, transform.position, Quaternion.identity);
            ore.GetComponent<WorldItem>().SetItem(ItemData, 1); 
          

            oreCount--;
            HitCount = MaxHitCount;

            
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

    public void Mine()
    {
        int pickaxePower = (int)StatSystem.instance.GetStat(StatsType.PickaxePower);
        float multiChance = StatSystem.instance.GetStat(StatsType.Multi_OreChance);
        bool shouldMulti = Random.value < multiChance;
        bool didDoubleHit = Random.value < 0.5;
        Debug.Log(didDoubleHit);

        // Tier 1: Insta-break (feels super powerful)
        if (pickaxePower >= MaxHitCount * 3)
        {
            SpawnOre(2, false); // Guaranteed 2 ores
            if (shouldMulti)
                SpawnOre(1, true); // Bonus ore
            StartCoroutine(RespawnIfDepleted());
            return;
        }

        // Tier 2: Medium break (1 hit = 2 HP)
        if (pickaxePower >= MaxHitCount * 2)
        {
       
            SpawnOre(1, false);
            if(didDoubleHit)
                SpawnOre(1, false);
            if (shouldMulti)
                SpawnOre(1, true);
            StartCoroutine(RespawnIfDepleted());
            return;
        }

        // Tier 3: Regular mining (needs multiple hits)
        HitCount -= pickaxePower;
        if (HitCount <= 0)
        {
            SpawnOre(1, false);
            if (shouldMulti)
                SpawnOre(1,true );
            StartCoroutine(RespawnIfDepleted());
            HitCount = MaxHitCount;
        }
    }

    void SpawnOre(int amount, bool ShouldMulti)
    {
        for (int i = 0; i < amount && oreCount > 0; i++)
        {
            GameObject ore = Instantiate(orePrefab, transform.position, Quaternion.identity);
            ore.GetComponent<WorldItem>().SetItem(ItemData, 1);
            Rigidbody2D rb = ore.AddComponent<Rigidbody2D>();
            if (rb != null)
                StartCoroutine(ApplyFloatDrop(rb));
            if(!ShouldMulti)
                oreCount--;
        }
    }

    IEnumerator RespawnIfDepleted()
    {
        if (oreCount <= 0)
            yield return StartCoroutine(RespawnOres());
    }
    public IEnumerator RespawnOres()
    {
        if (HasShadow)
            Shadow.SetActive(false);
        mineRenderer.enabled = false;
        col.enabled = false;
        yield return new WaitForSeconds(respawnTime);
        oreCount = Random.Range(minOres, maxOres);
        col.enabled = true;
        mineRenderer.enabled = true;
        if(HasShadow)
            Shadow.SetActive(true);
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


