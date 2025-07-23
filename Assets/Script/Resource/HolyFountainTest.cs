using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyFountainTest : ResourceNodeBase
{
    [SerializeField] private ItemData nonFilledGrail;
    [SerializeField] private ItemData filledGrail;
    [SerializeField] private GameObject grailPrefab;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float fadeSpeed = 1f;
    [SerializeField] private float maxDistance = 10f;

    private float currentVolume = 0f;
    private Transform player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        ControlSoundBasedOnDistance();
    }

    public override void Interact()
    {
        if (RemoveItemFromInventory(nonFilledGrail))
        {
            GameObject grail = Instantiate(grailPrefab, transform.position, Quaternion.identity);
            grail.GetComponent<WorldItem>().SetItem(filledGrail, 1);

            Rigidbody2D rb = grail.AddComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            StartCoroutine(ApplyFloatDrop(rb));
        }

        if (playerInput != null)
            playerInput.isInteracting = true;
    }

    private void ControlSoundBasedOnDistance()
    {
        if (audioSource == null || player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);
        float targetVolume = Mathf.Clamp01(1 - (distance / maxDistance));
        StartCoroutine(FadeSound(currentVolume, targetVolume));
    }

    private IEnumerator FadeSound(float fromVolume, float toVolume)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeSpeed)
        {
            currentVolume = Mathf.Lerp(fromVolume, toVolume, elapsedTime / fadeSpeed);
            audioSource.volume = currentVolume;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentVolume = toVolume;
        audioSource.volume = currentVolume;
    }
}
