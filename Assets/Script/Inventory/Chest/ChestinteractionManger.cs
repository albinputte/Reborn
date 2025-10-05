using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestinteractionManger : MonoBehaviour, IInteractable
{
    public ChestController controller;
    public List<InventoryItem> ItemToInitialize = new List<InventoryItem>();
    [SerializeField] public InteractableType Type;
    public InteractableType type { get => Type; set => Type = value; }
    [SerializeField] private Material NewMaterial;
    [SerializeField] private Material OldMaterial;
    [SerializeField] private GameObject Button;
    public SpriteRenderer ChestRenderer;
    private Animator animator;


    public void Start()
    {
        controller = FindAnyObjectByType<ChestController>();
        if(controller == null )
            SceneManger.instance.OnAllEssentialScenesLoaded += prepareRefrences;
        ChestRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    public void prepareRefrences()
    {
        controller = FindAnyObjectByType<ChestController>();
    }

    public void Interact()
    {
      

        if (controller.ChestUiIsActive)
        {
            controller.HideChest();
            animator.SetTrigger("ChestInteraction");
            UpdateItems();
        }
        else
        {
            if (TutorialManger.instance.EshouldAppear())
            {
                TutorialManger.instance.EwasInteractedWith(gameObject.name);
            }
            controller.PrepareChestData(ItemToInitialize, gameObject.name);
            controller.ShowChest();
            animator.SetTrigger("ChestInteraction");

        }
    }

    public void UpdateItems()
    {
        ItemToInitialize = controller.chestData.Inventory;
    }
    public void NearPlayer()
    {
        ChestRenderer.material = NewMaterial;
        if (TutorialManger.instance.EshouldAppear())
        {
        Button.gameObject.SetActive(true);
            }
    }

    public void LeavingPlayer()
    {
        ChestRenderer.material = OldMaterial;
        Button.gameObject.SetActive(false);
    }

}
