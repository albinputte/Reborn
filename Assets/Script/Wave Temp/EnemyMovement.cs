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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            IDamagable damagable = collision.GetComponentInChildren<IDamagable>();
          
            if (collision.gameObject.CompareTag("Player"))
            {
                Vector2 dir = (collision.transform.position - gameObject.transform.position).normalized;
                Debug.Log(damagable);
                if (damagable != null)
                    damagable.Hit(10, dir * 10);
            }
        }

    }

  

}
