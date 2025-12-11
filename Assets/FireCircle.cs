using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireCircle : MonoBehaviour
{
    
    public void DeactivateObject()
    {
        gameObject.transform.localScale = Vector3.zero;
        gameObject.SetActive(false);
  

    }

}
