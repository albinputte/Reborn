using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public static Respawn instance;
    public bool isRespawning;
    public Transform respawnPoint;
    public float respawnDelay = 3f;
    private SliderUi Healthui;
    private PlayerWeaponAgent agent;
    private void Awake()
    {
        instance = this;
        Healthui = FindAnyObjectByType<SliderUi>();
        agent = FindAnyObjectByType<PlayerWeaponAgent>();
    }

    public void RespawnPlayer(GameObject player)
    {
        StartCoroutine(RespawnCoroutine(player));
    }

    private IEnumerator RespawnCoroutine(GameObject player)
    {
        isRespawning = true;
        Collider2D[] collider = player.GetComponents<Collider2D>();
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        SpriteRenderer attackSprites = player.GetComponentInChildren<SpriteRenderer>();
        for (int i = 0; i < collider.Length; i++)
        {
            collider[i].enabled = false;
        }
        spriteRenderer.sortingLayerName = "Background";
        attackSprites.sortingLayerName = "Background";


        yield return new WaitForSeconds(respawnDelay);

        player.transform.position = respawnPoint.position;
        player.SetActive(true);

        
        Health health = player.GetComponentInChildren<Health>();
        if (health != null)
        {
            health.SetCurrentHealth((int)health.GetMaxHealth() / 2); 
            if(Healthui != null)
                Healthui.UpdateHealth();
            
        }

        for (int i = 0; i < collider.Length; i++)
        {
            collider[i].enabled = true;
        }

        spriteRenderer.sortingLayerName = "Foreground";
        attackSprites.sortingLayerName = "WeaponAnim";
        isRespawning = false;
        Debug.Log("Player respawned.");
    }
}