
using UnityEngine;

public class SoundWave : MonoBehaviour
{
    public float speed = 6f;
    public float lifetime = 2f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
    }
}
