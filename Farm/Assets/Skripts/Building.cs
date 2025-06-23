using System.Data.SqlTypes;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class Building : MonoBehaviour
{
    [SerializeField] private int price;
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite readySprite;

    private bool isBuild;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private UIManager uiManager;
    private Inventory inventory;

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = emptySprite;
        ChangeColider();
        uiManager = FindAnyObjectByType<UIManager>();
        inventory = FindAnyObjectByType<Inventory>();
    }

    public void RightClick() // Любой клик по зданию
    {

        if (isBuild && !animator.GetBool("Is Working"))
        {
            uiManager.Show(this);
        }
    }

    public void Click() // Любой клик по зданию
    {

        if (!isBuild )
        {
            Build();

        }

    }

    public void StartWork()
    {
        animator.SetBool("Is Working", true);
    }

    private void FinishWork()
    {
        inventory.UseMoney(1);
        animator.SetBool("Is Working", false);
    }

    #region Building
    private void Build() //Начало стройки, вычитание цены от суммы
    {
        if (inventory.CheckMoney() >= price)
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
