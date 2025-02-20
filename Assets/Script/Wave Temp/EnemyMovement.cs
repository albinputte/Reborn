using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float speed = 4f;

    private Vector2 position;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float move = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, move);
    }
}
