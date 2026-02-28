using System.Net.Mail;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Animal : MonoBehaviour
{

    [Header("Production")]
    [SerializeField] private List<Recipe> availableRecipes;
    [SerializeField] private int price;
    [SerializeField] private Sprite readySprite;

    private bool isBought = true; 

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private UIManager uiManager;
    private Inventory inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        ChangeColider();
        uiManager = UIManager.I;
        inventory = FindAnyObjectByType<Inventory>();
    }

    public void RightClick() // ����� ���� �� ������
    {
        Debug.Log("Right click on animal");

        if (isBought && !animator.GetBool("Is Working"))
        {
            Debug.Log("Show animal popup");
            uiManager.ShowAnimalPopUp(this);
        }
    }

    public List<Recipe> AnimalRecipes()
    {
        return availableRecipes;
    }

    public void Click() // ����� ���� �� ������
    {

        if (!isBought)
        {
            Buy();

        }

    }

    private void Buy()
    {
        if (inventory.UseMoney(0) >= price)
        {
            inventory.UseMoney(-price);
            isBought = true;
            ChangeColider();
        }
        else
        {
            Debug.Log("Not enough money to buy the animal.");
        }
    }

    private void ChangeColider()
    {
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();

        if (collider != null)
        {
            Destroy(collider); // ������� ������ �����
            gameObject.AddComponent<PolygonCollider2D>(); // �������� ����� �� ������ �������
        }
    }

    public void StartWork(Recipe recipe)
    {
        // 1) �����: ��������� ������� ������������
        if (!inventory.CheckRecipe(recipe))
        {
            Debug.LogWarning($"�� ������� ������������ ��� {recipe.recipeName}");
            return;
        }

        foreach (var ing in recipe.ingredients)
            inventory.RemoveItem(ing.item, ing.amount);
        animator.SetBool("Is Working", true);
    }

    public void FinishWork()
    {
        animator.SetBool("Is Working", false);
        spriteRenderer.sprite = readySprite;
        Debug.Log("-----------Finished!-------------");
 

    
}
}

