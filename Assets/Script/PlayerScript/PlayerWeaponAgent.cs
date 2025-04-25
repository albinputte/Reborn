using SmallHedge.SoundManager;
using System;
using System.Collections;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerWeaponAgent : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InventorySO inventory;
    [SerializeField] private WeaponItemData testWeapon;
    [SerializeField] private SpriteRenderer weaponSpriteRenderer;
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private Vector2[] faceDir;

    private BoxCollider2D[] attackColliders;
    private Sprite[] attackSprites;
    private SpriteRenderer playerSprite;
    private Animator attackAnimator;
    private WeaponAnimationHandler animationHandler;
    private Rigidbody2D rb;

    public PlayerInputManger playerInput;

    public WeaponItemData CurrentWeapon { get; private set; }
    public ItemData currentOrb;
    public WeaponInstances CurrentWeaponInstances { get; private set; }

    

    public int WeaponTypeIndex { get; private set; }
    public int CurrentAttackSpriteIndex { get; private set; }
    public bool IsAttackActive { get; private set; }

    private int tempFaceDir;

    public event Action OnEnter;
    public event Action OnExit;

    private int facingDirection;
    private int currentAttackIndex;
    private bool isHitStopActive;

    private void Awake()
    {
        InitializeComponents();
        WeaponInstances testinst = new WeaponInstances(testWeapon, null, 1212021);
        testinst.Weapon = testWeapon;
        SetWeapon(testinst);

        //CurrentWeapon = testWeapon;
        //attackSprites = CurrentWeapon.WeaponAttackSprites[0].AttackSprite;
    }

    private void InitializeComponents()
    {
        attackColliders = GetComponentsInChildren<BoxCollider2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        animationHandler = GetComponent<WeaponAnimationHandler>();
        attackAnimator = GetComponentInChildren<Animator>();
        rb = GetComponentInParent<Rigidbody2D>();

        if (attackColliders == null || attackColliders.Length == 0)
            Debug.LogError("No BoxCollider2D found in children!");
        if (!weaponSpriteRenderer)
            Debug.LogError("Weapon SpriteRenderer is missing!");
        if (!playerSprite)
            Debug.LogError("Player SpriteRenderer is missing!");
        if (!animationHandler)
            Debug.LogError("WeaponAnimationHandler is missing!");
        if (!attackAnimator)
            Debug.LogError("AttackAnimator is missing!");
    }

    public void SetWeapon(WeaponInstances newWeapon)
    {
        if (CurrentWeapon != null)
            inventory.AddItem(CurrentWeapon, 1, CurrentWeaponInstances);
        CurrentWeaponInstances = newWeapon;
        CurrentWeapon = newWeapon.Weapon;
        currentOrb = newWeapon.GetOrb();
        WeaponTypeIndex = (int)CurrentWeapon.WeaponType;
        attackSprites = CurrentWeapon.WeaponAttackSprites[0].AttackSprite;

    }

    public void Activate(int direction)
    {
        if (IsAttackActive) return;
       
        IsAttackActive = true;
        weaponSpriteRenderer.enabled = true;
        tempFaceDir = direction;
        facingDirection = TemporaryDirCorrection(direction);
      
   
        string animName = GetAnimationName(CurrentWeapon, facingDirection, currentAttackIndex);
        attackAnimator.Play(animName);

        OnEnter?.Invoke();
        SoundManager.PlaySound(CurrentWeapon.attackSounds[currentAttackIndex]);

        currentAttackIndex++;
    }

    public void Deactivate()
    {
        weaponSpriteRenderer.enabled = false;
        IsAttackActive = false;
        facingDirection = -1;
        OnExit?.Invoke();
    }

    public void EnableCollider()
    {
        if (facingDirection < 0 || facingDirection >= attackColliders.Length) return;

        attackColliders[facingDirection].enabled = true;
       
    }

    public void DisableCollider()
    {
        if (facingDirection < 0 || facingDirection >= attackColliders.Length) return;

        attackColliders[facingDirection].enabled = false;
       
    }

    public void SetPlayerVelocityZero()
    {
        StartCoroutine(LerpVel());
    }

    public IEnumerator LerpVel()
    {
        Vector2 x = rb.velocity / 10;
        for (int i = 0;i < 5; i++)
        {
            rb.velocity -= x;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void ApplyKnockbackToPlayer()
    {
        rb.AddForce(faceDir[tempFaceDir] * 0.4f, ForceMode2D.Impulse);
    }

    public void UpdateWeaponSprite(SpriteRenderer _)
    {
        if (CurrentAttackSpriteIndex < attackSprites.Length)
        {
            Debug.Log(attackSprites[CurrentAttackSpriteIndex]);
            weaponSpriteRenderer.sprite = attackSprites[CurrentAttackSpriteIndex];
            CurrentAttackSpriteIndex++;
        }
    }

    private void HandleEnter()
    {
        playerSprite.RegisterSpriteChangeCallback(UpdateWeaponSprite);
    }

    private void HandleExit()
    {
        IsAttackActive = false;
        playerSprite.UnregisterSpriteChangeCallback(UpdateWeaponSprite);
        attackAnimator.Play("EmptyAnim");
    }

    private void OnEnable()
    {
        animationHandler.OnAnimationComplete += Deactivate;
        OnEnter += HandleEnter;
        OnExit += HandleExit;
    }

    private void OnDisable()
    {
        animationHandler.OnAnimationComplete -= Deactivate;
        OnEnter -= HandleEnter;
        OnExit -= HandleExit;
    }

    private string GetAnimationName(WeaponItemData weapon, int direction, int index)
    {
        return $"{weapon.WeaponType}_Facing{direction}_Nr{index + 1}";
    }

    public void StartAttackResetTimer()
    {
        StartCoroutine(ResetAttackTimer(1f));
    }

    private IEnumerator ResetAttackTimer(float delay)
    {
        if (currentAttackIndex >= 3)
        {
            currentAttackIndex = 0;
            CurrentAttackSpriteIndex = 0;
            yield break;
        }

        int cachedAttackIndex = currentAttackIndex;
        yield return new WaitForSeconds(delay);

        if (cachedAttackIndex == currentAttackIndex)
        {
            currentAttackIndex = 0;
            CurrentAttackSpriteIndex = 0;
        }
    }

    public void EditSortingLayer(int index)
    {
        weaponSpriteRenderer.sortingOrder = index;
    }

    public void TriggerHitStop()
    {
        if (isHitStopActive) return;

        Time.timeScale = 0.1f;
        StartCoroutine(ResetTimeScaleAfterDelay(0.1f));
    }

    private IEnumerator ResetTimeScaleAfterDelay(float delay)
    {
        isHitStopActive = true;
        yield return new WaitForSecondsRealtime(delay);
        Time.timeScale = 1f;
        isHitStopActive = false;
    }


    private int TemporaryDirCorrection(int dir)
    {
        switch (dir)
        {
            case 0:
                PlayerTransform.localScale = new Vector3(1, 1, 0);
                return 0;

            case 1:
                return 1;

            case 2:
                return 2;

            case 3:
                PlayerTransform.localScale = new Vector3(-1, 1, 0);
                return 0;

            case 4:
                PlayerTransform.localScale = new Vector3(1, 1, 0);
                return 0;
            case 5:
                PlayerTransform.localScale = new Vector3(1, 1, 0);
                return 0;

            case 6:
                PlayerTransform.localScale = new Vector3(-1, 1, 0);
                return 0;
            case 7:
                PlayerTransform.localScale = new Vector3(-1, 1, 0);
                return 0;

            default:
                return 0;
        }
    }
}
