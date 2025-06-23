using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class Building : MonoBehaviour
{
    [Header("Production")]
    [Tooltip("�������, ������� ����� ��������� ��� ������")]
    public List<Recipe> availableRecipes;

    [SerializeField] private int price;
    [SerializeField] private Sprite readySprite;

    private bool isBuild;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private UIManager uiManager;
    private Inventory inventory;
    private Recipe currentRecipe;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChangeColider();
        uiManager = FindAnyObjectByType<UIManager>();
        inventory = FindAnyObjectByType<Inventory>();
    }

    public void RightClick() // ����� ���� �� ������
    {

        if (isBuild && !animator.GetBool("Is Working"))
        {
            uiManager.Show(this);
        }
    }

    public void Click() // ����� ���� �� ������
    {

        if (!isBuild )
        {
            Build();

        }

    }

    public void StartWork(Recipe recipe)
    {
        // 1) �����: ��������� ������� ������������
        if (!inventory.HasIngredients(recipe))
        {
            Debug.LogWarning($"�� ������� ������������ ��� {recipe.recipeName}");
            return;
        }

        // 2) ��������� �����������
        foreach (var ing in recipe.ingredients)
            inventory.RemoveItem(ing.item, ing.amount);

        // 3) ��������� �������� ������
        animator.SetBool("Is Working", true);
        currentRecipe = recipe;
    }

    private void FinishWork()
    {
        inventory.AddItem(currentRecipe.result, currentRecipe.resultCount);
        animator.SetBool("Is Working", false);

        Debug.Log("-----------Finished!-------------");
    }

    #region Building
    private void Build() //������ �������, ��������� ���� �� �����
    {
        if (inventory.CheckMoney() >= price)
        {
            inventory.UseMoney(-price);
            animator.SetTrigger("Start Building");

        }
    }

    public void FinishBuilding() //���������� ������� ������ ����, ��� ������ �������
    {

        animator.SetTrigger("Finish Building");
        isBuild = true;
        spriteRenderer.sprite = readySprite;
        ChangeColider();

    }

    private void ChangeColider()  //����� ���������� ��� ����� �������
    {
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();

        if (collider != null)
        {
            Destroy(collider); // ������� ������ �����
            gameObject.AddComponent<PolygonCollider2D>(); // �������� ����� �� ������ �������
        }
    }

    #endregion Building



}
