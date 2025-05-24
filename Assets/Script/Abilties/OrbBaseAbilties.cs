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

        // Normalize angle to 0–360 for direction logic
        if (angle < 0)
            angle += 360;

        // CorrectedAngle is ONLY for sprite rotation
        float correctedAngle = angle - 90;
        Quaternion rotation = Quaternion.Euler(0, 0, correctedAngle);

        // Get direction from unmodified angle
        Directions direction = GetDirectionFromAngle(angle);

        // Offset by direction
        (float offsetX, float offsetY) = direction switch
        {
            Directions.Up => (0f, 0.4f),
            Directions.Down => (0f, -0.4f),
            Directions.Left => (-0.3f, 0f),
            Directions.Right => (0.3f, 0f),
            Directions.LeftUp => (-0.3f, 0.3f),
            Directions.RightUp => (0.3f, 0.3f),
            Directions.LeftDown => (-0.3f, -0.3f),
            Directions.RightDown => (0.3f, -0.3f),
            _ => (0f, 0f)
        };

        Debug.Log($"Angle: {angle}°, Direction: {direction}");

        Vector2 spawnPos = new Vector2(Character.transform.position.x + offsetX, Character.transform.position.y + offsetY);
        GameObject obj = GameObject.Instantiate(Prefab, spawnPos, rotation);
        GameObject.Destroy(obj, 0.3f);

        Rigidbody2D rb = obj.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.AddForce(lookDir * Force, ForceMode2D.Impulse);

        Debug.Log("Fireball activated for " + damage + " damage!");
    }
    private Directions GetDirectionFromAngle(float angle)
    {
        foreach (var (min, max, dir) in angleToDirectionMap)
        {
            if (min < max)
            {
                if (angle >= min && angle < max)
                    return dir;
            }
            else
            {
                // Handle wrap-around (e.g., 337.5 to 360 and 0 to 22.5)
                if (angle >= min || angle < max)
                    return dir;
            }
        }
        return Directions.Right; // Default fallback
    }
    private static readonly (float min, float max, Directions dir)[] angleToDirectionMap = new[]
{
    (337.5f, 360f, Directions.Right),
    (0f, 22.5f, Directions.Right),
    (22.5f, 67.5f, Directions.RightUp),
    (67.5f, 112.5f, Directions.Up),
    (112.5f, 157.5f, Directions.LeftUp),
    (157.5f, 202.5f, Directions.Left),
    (202.5f, 247.5f, Directions.LeftDown),
    (247.5f, 292.5f, Directions.Down),
    (292.5f, 337.5f, Directions.RightDown)
};
}
