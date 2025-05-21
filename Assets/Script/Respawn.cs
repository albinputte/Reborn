using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public static Respawn instance;

    public Transform respawnPoint;
    public float respawnDelay = 3f;
    private SliderUi Healthui;
    private void Awake()
    {
        instance = this;
        Healthui = FindAnyObjectByType<SliderUi>();
    }

    public void RespawnPlayer(GameObject player)
    {
        StartCoroutine(RespawnCoroutine(player));
    }

    private IEnumerator RespawnCoroutine(GameObject player)
    {
        player.SetActive(false);
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

        Debug.Log("Player respawned.");
    }
}