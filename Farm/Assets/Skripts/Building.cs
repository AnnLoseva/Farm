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


    public int Click(int money) // ����� ���� �� ������
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

    private int Build(int money) //������ �������, ��������� ���� �� �����
    {
        if (money >= price)
        {
            Debug.Log(money);
            money -= price;
            animator.Play("Building Works");

        }

        return money;

    }

    public void FinishBuilding() //���������� ������� ������ ����, ��� ������ �������
    {

        animator.Play("Idle");
        isBuild = true;
        spriteRenderer.sprite = readySprite;
        ChangeColider();

    }

    private int Destroy(int money) // ���������� �������, ����������� �����
    {
        money += price;
        isBuild = false;
        spriteRenderer.sprite = emptySprite;
        ChangeColider();

        return money; 
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
}
