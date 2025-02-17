using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedScript : MonoBehaviour
{

    [SerializeField] Sprite GrowState1;
    [SerializeField] Sprite GrowState2;
    [SerializeField] SpriteRenderer SpriteR;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Grow());
        
    }

    IEnumerator Grow()
    {
        yield return new WaitForSeconds(10);
        SpriteR.sprite = GrowState1;
        yield return new WaitForSeconds(10);
        SpriteR.sprite = GrowState2;
       
    }


}
