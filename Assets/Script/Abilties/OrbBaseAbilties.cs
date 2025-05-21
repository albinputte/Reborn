using UnityEngine;
using System;
using Cinemachine.Utility;

public class BaseOrbAbilties : MonoBehaviour
{
    [SerializeReference, SubclassSelector]
    public BaseOrbAbilties OrbAbilties;
}

[Serializable] // ✅ Required for managed reference serialization
public abstract class BaseAbiltiesOrb 
{
    public abstract void ActivateAbilties(GameObject Character);
}

[Serializable] // ✅ Required for subclasses
public class OrbTest : BaseAbiltiesOrb
{
    public override void ActivateAbilties(GameObject Character)
    {
        Debug.Log("Test ability activated");
    }
}
[Serializable]
public class OrbFireball : BaseAbiltiesOrb
{
    
    public int damage = 10;
    public GameObject Prefab;
    public float Force;
    public override void ActivateAbilties(GameObject Character)
    {
        Vector3 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        dir.z = 0;
        Vector3 lookDir = (dir - Character.transform.position).normalized;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg; 

        Quaternion rotation = Quaternion.Euler(0, 0, angle -90);
        Debug.Log("Rotation is " + dir);
        GameObject obj = GameObject.Instantiate(Prefab, Character.transform.position, rotation);
        Debug.Log("Rotation is " + rotation);
        GameObject.Destroy(obj, 0.3f);
        Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.AddForce(lookDir * Force, ForceMode2D.Impulse);


        Debug.Log("Fireball activated for " + damage + " damage!");
    }
}
