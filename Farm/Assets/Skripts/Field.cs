using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Animator))]
public class Field : MonoBehaviour
{
    [Header("Field Settings")]
    [SerializeField] private int price = 20;
    [SerializeField] private Sprite readySprite;

    private bool isBuilt;
    private bool isPlanted;
    private bool isReady;

    private SpriteRenderer spriteRenderer;
    private UIManager uiManager;
    private Inventory inventory;
    private Seed currentSeed;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        uiManager = UIManager.I;
        inventory = FindAnyObjectByType<Inventory>();
        UpdateCollider();
    }

    public void RightClick()
    {
        if (isBuilt && !isPlanted && !isReady)
            uiManager.ShowFieldPopUp(this);
    }

    public void Click()
    {
        if (!isBuilt)
        {
            TryBuild();
        }
        else if (isReady)
        {
            Harvest();
        }
    }

    private void TryBuild()
    {
        if (inventory.UseMoney(0) >= price)
        {
            inventory.UseMoney(-price);
            FinishBuilding();
        }
    }

    public void FinishBuilding()
    {
        isBuilt = true;
        spriteRenderer.sprite = readySprite;
        animator.SetTrigger("Buy");
        UpdateCollider();
    }

    private void UpdateCollider()
    {
        PolygonCollider2D col = GetComponent<PolygonCollider2D>();
        if (col != null) Destroy(col);
        gameObject.AddComponent<PolygonCollider2D>();
    }

    public List<Seed> AllSeeds()
    {
        return inventory.GetAvailableSeeds();
    }

    public void Plant(Seed seed)
    {
        if (!inventory.HasSeed(seed)) return;

        inventory.UseSeed(seed);
        currentSeed = seed;
        isPlanted = true;
        isReady = false;

        animator.SetInteger("GrowthID", seed.growthAnimationID);
        animator.SetBool("IsGrowing", true);
    }

    public void OnGrowthComplete()
    {
        isReady = true;
        animator.SetBool("IsGrowing", false);
    }

    private void Harvest()
    {
        for (int i = 0; i < currentSeed.yieldCount; i++)
            inventory.AddItem(currentSeed.cropResult);

        isPlanted = false;
        isReady = false;
        animator.SetTrigger("Harvest");
    }

    public bool IsBuilt() => isBuilt;
}
