using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerryBushTest : ResourceNodeBase
{
    [SerializeField] private int berryCount = 3;
    [SerializeField] private GameObject berryPrefab;
    [SerializeField] private ItemData itemData;
    [SerializeField] private Sprite[] bushSprites; // 0 = full, 1 = empty
    [SerializeField] private float spawnTime = 10f;
    [SerializeField] private int minBerries = 1;
    [SerializeField] private int maxBerries = 4;

    protected override void Awake()
    {
        base.Awake();
        berryCount = Random.Range(minBerries, maxBerries);
    }

    public override void Interact()
    {
        if (berryCount <= 0) return;

        if (TutorialManger.instance.EshouldAppear())
        {
            TutorialManger.instance.EwasInteractedWith(gameObject.name);
        }

        SoundManager.PlaySound(SoundType.InteractBerryBush);

        GameObject berry = Instantiate(berryPrefab, transform.position, Quaternion.identity);
        berry.GetComponent<WorldItem>().SetItem(itemData, 1);

        Rigidbody2D rb = berry.AddComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        StartCoroutine(ApplyFloatDrop(rb));

        berryCount--;

        if (berryCount == 0)
        {
            spriteRenderer.sprite = bushSprites[1];
            StartCoroutine(RespawnBerries());
        }
    }

    private IEnumerator RespawnBerries()
    {
        yield return new WaitForSeconds(spawnTime);
        berryCount = Random.Range(minBerries, maxBerries);
        spriteRenderer.sprite = bushSprites[0];
    }
}
