using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BerryBush : MonoBehaviour, IInteractable
{
    public int berryCount = 3;
    public GameObject berryPrefab;
    [SerializeField] private ItemData ItemData;
    [SerializeField] private Sprite[] BushSprites;
    private SpriteRenderer BushRenderer;
    [SerializeField ] private float SpawnTime;
    [SerializeField] private int MinBerries;
    [SerializeField] private int MaxBerries;
    [SerializeField] private Material NewMaterial;
    [SerializeField] private Material OldMaterial;
    [SerializeField] private GameObject Button;


   [SerializeField]public InteractableType Type;
    public InteractableType type { get => Type; set => Type = value; }

    public void Start()
    {
        BushRenderer = GetComponent<SpriteRenderer>();
        berryCount = Random.Range(MinBerries, MaxBerries);
    }

    public void Interact()
    {
        if (berryCount > 0)
        {
            if (TutorialManger.instance.EshouldAppear())
            {
                TutorialManger.instance.EwasInteractedWith(gameObject.name);
            }
            GameObject obj = Instantiate(berryPrefab, transform.position, Quaternion.identity);
            SoundManager.PlaySound(SoundType.InteractBerryBush);
            obj.GetComponent<WorldItem>().SetItem(ItemData, 1);
            berryCount--;
            Debug.Log("Picked a berry! Berries left: " + berryCount);

      
            Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (rb != null)
                StartCoroutine(ApplyFloatDrop(rb));
            if (berryCount == 0)
                StartCoroutine(SpawnNewBerries());
        }
     
    }

    public IEnumerator SpawnNewBerries()
    {
         BushRenderer.sprite = BushSprites[1];
        yield return new WaitForSeconds(SpawnTime);
        berryCount = Random.Range(MinBerries, MaxBerries);
        BushRenderer.sprite = BushSprites[0];
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
        rb.gravityScale = 0f; rb.velocity = Vector2.zero;
    }
    public void NearPlayer()
    {
        BushRenderer.material = NewMaterial;
        if (TutorialManger.instance.EshouldAppear())
        {
            Button.gameObject.SetActive(true);
        }
    }

    public void LeavingPlayer()
    {
        BushRenderer.material = OldMaterial;
        Button.gameObject.SetActive(false);
    }
}
