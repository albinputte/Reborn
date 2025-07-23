using SmallHedge.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinnableOreTest : ResourceNodeBase
{
    [SerializeField] private GameObject orePrefab;
    [SerializeField] private ItemData itemData;
    [SerializeField] private int maxHitCount = 3;
    [SerializeField] private float respawnTime = 10f;
    [SerializeField] private int minOres = 1;
    [SerializeField] private int maxOres = 5;
    [SerializeField] private GameObject pickaxeHint;
    [SerializeField] private GameObject stoneParticles;
    [SerializeField] private GameObject shadow;
    [SerializeField] private bool isStone = false;

    private int oreCount;
    private int hitCount;
    private Collider2D[] colliders;

    protected override void Awake()
    {
        base.Awake();
        colliders = GetComponents<Collider2D>();
        ResetOre();
    }

    private void ResetOre()
    {
        oreCount = Random.Range(minOres, maxOres);
        hitCount = maxHitCount;
    }

    public override void Interact()
    {
        if (oreCount <= 0)
        {
            Debug.Log("The mine is empty.");
            return;
        }

        if (TutorialManger.instance.EshouldAppear())
        {
            TutorialManger.instance.EwasInteractedWith(gameObject.name);
        }

        if (type == InteractableType.Minning)
        {
            SoundManager.PlaySound(SoundType.PickAxe_Sound);
            Instantiate(stoneParticles, transform.position, Quaternion.identity);
            Mine();

            if (!TutorialManger.instance.PickaxeShouldAppear() && isStone)
                TutorialManger.instance.StoneMined();

            if (hitCount > 0)
                return;
        }

        SpawnOreItem(1);
        oreCount--;
        hitCount = maxHitCount;

        if (oreCount == 0)
            StartCoroutine(RespawnOre());
    }

    private void Mine()
    {
        int pickaxePower = (int)StatSystem.instance.GetStat(StatsType.PickaxePower);
        float multiChance = StatSystem.instance.GetStat(StatsType.Multi_OreChance);
        bool multiDrop = Random.value < multiChance;
        bool doubleHit = Random.value < 0.5f;

        // Tier 1: Insta-break
        if (pickaxePower >= maxHitCount * 3)
        {
            SpawnOreItem(2);
            if (multiDrop) SpawnOreItem(1);
            StartCoroutine(RespawnIfEmpty());
            return;
        }

        // Tier 2: Medium break
        if (pickaxePower >= maxHitCount * 2)
        {
            SpawnOreItem(1);
            if (doubleHit) SpawnOreItem(1);
            if (multiDrop) SpawnOreItem(1);
            StartCoroutine(RespawnIfEmpty());
            return;
        }

        // Tier 3: Normal mining
        hitCount -= pickaxePower;
        if (hitCount <= 0)
        {
            SpawnOreItem(1);
            if (multiDrop) SpawnOreItem(1);
            StartCoroutine(RespawnIfEmpty());
            hitCount = maxHitCount;
        }
    }

    private void SpawnOreItem(int count)
    {
        for (int i = 0; i < count && oreCount > 0; i++)
        {
            GameObject ore = Instantiate(orePrefab, transform.position, Quaternion.identity);
            ore.GetComponent<WorldItem>().SetItem(itemData, 1);
            Rigidbody2D rb = ore.AddComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            StartCoroutine(ApplyFloatDrop(rb));
            oreCount--;
        }
    }

    private IEnumerator RespawnIfEmpty()
    {
        if (oreCount <= 0)
            yield return StartCoroutine(RespawnOre());
    }

    private IEnumerator RespawnOre()
    {
        if (shadow != null) shadow.SetActive(false);
        spriteRenderer.enabled = false;

        foreach (var col in colliders)
            col.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        ResetOre();
        spriteRenderer.enabled = true;
        foreach (var col in colliders)
            col.enabled = true;

        if (shadow != null) shadow.SetActive(true);
    }

    public override void NearPlayer()
    {
        base.NearPlayer();

        if (!TutorialManger.instance.PickaxeShouldAppear() && isStone && pickaxeHint != null)
            pickaxeHint.SetActive(true);
    }

    public override void LeavingPlayer()
    {
        base.LeavingPlayer();
        if (isStone && pickaxeHint != null)
            pickaxeHint.SetActive(false);
    }
}