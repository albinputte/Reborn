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
    private GameObject Player;
    private PlayerWeaponAgent agent;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        Healthui = FindAnyObjectByType<SliderUi>();
        agent = FindAnyObjectByType<PlayerWeaponAgent>();
    }

    public void RespawnPlayer(GameObject player)
    {
        isRespawning = true;
        Player = player;
        PlayerWeaponAgent.Instance.OnRespawn();
        Collider2D[] collider = player.GetComponents<Collider2D>();
        for (int i = 0; i < collider.Length; i++)
        {
            collider[i].enabled = false;
        }
    }

    public void RespawnDone()
    {
       
  


        Health health = Player.GetComponentInChildren<Health>();
        if (health != null)
        {
            health.SetCurrentHealth((int)health.GetMaxHealth() / 2);
            if (Healthui != null)
                Healthui.UpdateHealth();

        }
        Collider2D[] collider = Player.GetComponents<Collider2D>();
        for (int i = 0; i < collider.Length; i++)
        {
            collider[i].enabled = true;
        }


        isRespawning = false;
    }

    public void MoveCharacterToRespawn()
    {
        Player.transform.position = respawnPoint.position;
    }

 
}