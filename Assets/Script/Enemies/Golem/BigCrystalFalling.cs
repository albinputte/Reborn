using System.Collections;
using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

public class BigCrystalFalling : MonoBehaviour
{
    public float fallSpeed = 15f;
    public int damage = 20;
    public float damageRadius = 1.5f;

    private Vector3 targetPosition;
    private GameObject shadow;
    private CreaturePillar pillar;
    private bool HasImpact = false;
    [SerializeField] private GameObject hitSplash;
    private SpriteRenderer renderer;
    private Animator animator;
    public Sprite GroundSprite;

    public void Init(Vector3 targetPos, GameObject shadowObj, CreaturePillar Pilar)
    {
        targetPosition = targetPos;
        shadow = shadowObj;
        pillar = Pilar;
    }
    public void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            fallSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetPosition) <= 0.05f && !HasImpact)
        {
            Impact();
        }
    }

    void Impact()
    {
        // Damage in radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            targetPosition,
            damageRadius
        );
        hitSplash.SetActive( true );
        animator.Play("BigCrystalGround");

        foreach (var hit in hits)
        {
            IDamagable damagable = hit.GetComponent<IDamagable>();
            if (damagable != null)
            {
                //damagable.Hit(damage, Vector2.zero);
            }
            else
            {
                damagable = hit.GetComponentInChildren<IDamagable>();
                if (damagable != null)
                {
                    damagable.Hit(damage, Vector2.zero);
                }
            }

        }

        if (shadow != null)
            Destroy(shadow);
        if (pillar.StoneList.ContainsKey(transform.position))
        {
            Destroy(pillar.StoneList[transform.position]);
            pillar.StoneList[transform.position] = gameObject;
        }
        else {
            pillar.StoneList[transform.position] = gameObject;
        }


        HasImpact = true;


    }

    private void OnDestroy()
    {
        pillar.StoneList.Remove(transform.position);
    }
    public void StartRoots()
    {

        animator.Play("BigCrystalGroundRoots");
    }

    public void StartDestroy()
    {
        animator.Play("BigCrystalGroundBreak");
    }


#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
#endif
}
