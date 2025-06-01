using System.Data.SqlTypes;
using System.Diagnostics;
using Unity.VisualScripting;
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

    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = emptySprite;
        ChangeColider();
    }


    public int Click(int money) // Любой клик по зданию
    {

        if (!isBuild )
        {
            money = Build(money);

        }
        else if(isBuild)
        {
            money = Destroy(money);
        }

        return money;
    }

    private int Build(int money) //Начало стройки, вычитание цены от суммы
    {
        if (money >= price)
        {
            Debug.Log(money);
            money -= price;
            animator.Play("Building Works");

        }

        return money;

    }

    public void FinishBuilding() //Завершение стройки послек того, как прошла стройка
    {

        animator.Play("Idle");
        isBuild = true;
        spriteRenderer.sprite = readySprite;
        ChangeColider();

    }

    private int Destroy(int money) // Разрушение объекта, возвращение денег
    {
        money += price;
        isBuild = false;
        spriteRenderer.sprite = emptySprite;
        ChangeColider();

        return money; 
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
}
