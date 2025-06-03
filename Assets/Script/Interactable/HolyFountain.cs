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
    [SerializeField] private Transform Player;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float fadeSpeed = 1f;   // Speed at which the sound fades in
    [SerializeField] private float maxDistance = 10f; // Max distance at which sound starts to fade in

    private float currentVolume = 0f;
    public void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Call the method to control sound volume based on distance
        ControlSoundBasedOnDistance();
    }

    public void Interact()
    {
        if (SearchForGrail(NonFilledGrail))
        {
            GameObject ore = Instantiate(GrailPrefab, transform.position, Quaternion.identity);
            ore.GetComponent<WorldItem>().SetItem(FilledGrail, 1);

            Rigidbody2D rb = ore.AddComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            if (rb != null)
                StartCoroutine(ApplyFloatDrop(rb));
        }
    }

    public void LeavingPlayer()
    {
        FountainRenderer.material = NewMaterial;
        // Optionally stop the sound when the player leaves
       
    }

    public void NearPlayer()
    {
        FountainRenderer.material = OldMaterial;
        // Fade in sound when the player is near
       
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

    // Method to fade the sound in based on player proximity
    private void ControlSoundBasedOnDistance()
    {
        if (audioSource != null)
        {
            // Get distance between player (main camera) and fountain
            float distance = Vector3.Distance(Player.position, transform.position);

            // Calculate the target volume based on the distance (fading in as player gets closer)
            float targetVolume = Mathf.Clamp01(1 - (distance / maxDistance)); // Max volume when close, 0 when far

            // Smoothly fade the sound volume to the target volume
            StartCoroutine(FadeSound(currentVolume, targetVolume));
        }
    }

    // Coroutine to handle the gradual fade of the sound
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

        // Ensure the target volume is set correctly at the end of the fade
        currentVolume = toVolume;
        audioSource.volume = currentVolume;
    }
}




