using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    public Transform miniMapCamera;
    public Transform PlayerTransform;
    void Start()
    {
        miniMapCamera = GetComponent<Transform>();
        TryFindPlayer();
    }

    private void TryFindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            PlayerTransform = playerObj.transform;
        }
        else
        {
            SceneManger.instance.OnAllEssentialScenesLoaded += PrepareReferences;

        }
    }

    public void PrepareReferences()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(miniMapCamera != null && PlayerTransform != null)
            transform.position = new Vector3(PlayerTransform.position.x, PlayerTransform.position.y, -10);
    }
}
