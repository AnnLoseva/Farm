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

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = emptySprite;
        ChangeColider();
        uiManager = FindAnyObjectByType<UIManager>();
    }

    public int RightClick(int money) // Любой клик по зданию
    {

        if (!isBuild)
        {
            

        }
        else if (isBuild && !animator.GetBool("Is Working"))
        {
            uiManager.Show(this);
        }

        return money;
    }

    public int Click(int money) // Любой клик по зданию
    {

        if (!isBuild )
        {
            money = Build(money);

        }
        else if(isBuild && !animator.GetBool("Is Working"))
        {
        }

        return money;
    }

    public void StartWork()
    {
        Debug.Log("---------------------Working!!!!--------------------");
        animator.SetBool("Is Working", true);
    }

    private void FinishWork()
    {
        Debug.Log("-------------Finished!!!!-------------------");
        animator.SetBool("Is Working", false);
    }

    #region Building
    private int Build(int money) //Начало стройки, вычитание цены от суммы
    {
        if (money >= price)
        {
            Debug.Log(money);
            money -= price;
            animator.SetTrigger("Start Building");

        }

        return money;

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
