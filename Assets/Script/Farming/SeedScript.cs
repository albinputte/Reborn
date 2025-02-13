using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedScript : MonoBehaviour
{

    [SerializeField] Sprite Carrot;
    [SerializeField] SpriteRenderer SpriteR;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Grow());
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Grow()
    {
        yield return new WaitForSeconds(10);
        SpriteR.sprite = Carrot;
        transform.localScale = new Vector3(0.5f, 0.5f, 1);
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }


}
