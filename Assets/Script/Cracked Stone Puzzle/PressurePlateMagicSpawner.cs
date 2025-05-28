using SmallHedge.SoundManager;
using UnityEngine;

public class PressurePlateMagicSpawner : MonoBehaviour
{
    [Header("Magic Sphere Settings")]
    public GameObject magicSpherePrefab;
    public Transform spawnPoint;

    [Header("Sprite Settings")]
    public Sprite pressedSprite;
    public Sprite defaultSprite;

    private SpriteRenderer spriteRenderer;
    private GameObject spawnedMagicSphere;
    private bool isPressed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = defaultSprite;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isPressed && other.CompareTag("Player"))
        {
            if (spawnedMagicSphere == null)
            {
                SoundManager.PlaySound(SoundType.Preasure_Plate);
                // Spawn the magic sphere
                spawnedMagicSphere = Instantiate(magicSpherePrefab, spawnPoint.position, Quaternion.identity);
                // Assign reference to this pressure plate
                MagicSphere sphereScript = spawnedMagicSphere.GetComponent<MagicSphere>();
                if (sphereScript != null)
                {
                    sphereScript.originPlate = this;
                }
                // Change sprite to indicate pressed
                spriteRenderer.sprite = pressedSprite;
                isPressed = true;
            }
        }
    }

    public void OnMagicSphereDestroyed()
    {
        spawnedMagicSphere = null;
        spriteRenderer.sprite = defaultSprite;
        isPressed = false;
    }

}
