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
    [Tooltip("Рецепты, которые может выполнять это здание")]
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

    public void RightClick() // Любой клик по зданию
    {

        if (isBuild && !animator.GetBool("Is Working"))
        {
            uiManager.ShowPopUp(this);
        }
    }

    public void Click() // Любой клик по зданию
    {

        if (!isBuild )
        {
            Build();

        }

    }

    public void StartWork(Recipe recipe)
    {
        // 1) Новое: проверяем наличие ингредиентов
        if (!inventory.CheckRecipe(recipe))
        {
            Debug.LogWarning($"Не хватает ингредиентов для {recipe.recipeName}");
            return;
        }

        // 2) Списываем ингредиенты
        foreach (var ing in recipe.ingredients)
            inventory.RemoveItem(ing.item, ing.amount);

        // 3) Запускаем анимацию работы
        animator.SetBool("Is Working", true);
        currentRecipe = recipe;
    }

    private void FinishWork()
    {
        for (int i = 0; i < currentRecipe.resultCount; i++)
        {
            inventory.AddItem(currentRecipe.result);

        }
        animator.SetBool("Is Working", false);

        Debug.Log("-----------Finished!-------------");
    }

    #region Building
    private void Build() //Начало стройки, вычитание цены от суммы
    {
        if (inventory.UseMoney(0) >= price)
        {
            inventory.UseMoney(-price);
            animator.SetTrigger("Start Building");

        }
    }

    public void FinishBuilding() //Завершение стройки послек того, как прошла стройка
    {

        animator.SetTrigger("Finish Building");
        isBuild = true;
        spriteRenderer.sprite = readySprite;
        ChangeColider();

    }

    private void ChangeColider()  //Смена коллайдера при смене спрайта
    {
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();

        if (collider != null)
        {
            Destroy(collider); // удалить старую форму
            gameObject.AddComponent<PolygonCollider2D>(); // добавить новую по новому спрайту
        }
    }

    #endregion Building



}
