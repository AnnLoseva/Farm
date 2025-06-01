using System.Data.SqlTypes;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class Building : MonoBehaviour
{
    [SerializeField] private int price;
    [SerializeField] float buildTime;
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite buildingSprite;
    [SerializeField] private Sprite readySprite;

    private bool isBuilding;
    private bool isBuild;
    private SpriteRenderer spriteRenderer;
    private float currentBuildingTime;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = emptySprite;
        ChangeColider();
    }

    private void Update()
    {
        if (isBuilding)
        {
            currentBuildingTime += Time.deltaTime;
            if(currentBuildingTime > buildTime)
            {
                FinishBuilding();
            }
        }
    }

    public int Click(int money)
    {

        if (!isBuild & !isBuilding)
        {
            money = Build(money);

        }
        else if(isBuild)
        {
            money = Destroy(money);
        }

        return money;
    }

    private int Build(int money)
    {
        if (money >= price)
        {
            Debug.Log(money);
            money -= price;
            spriteRenderer.sprite = buildingSprite;
            isBuilding = true;
            

        }

        return money;

    }

    private void FinishBuilding()
    {
        isBuilding = false;
        currentBuildingTime = 0;
        isBuild = true;
        spriteRenderer.sprite = readySprite;
        ChangeColider();
     
    }

    private int Destroy(int money)
    {
        money += price;
        isBuild = false;
        spriteRenderer.sprite = emptySprite;
        ChangeColider();

        return money; 
    }

    private void ChangeColider()
    {
        PolygonCollider2D collider = GetComponent<PolygonCollider2D>();

        if (collider != null)
        {
            Destroy(collider); // удалить старую форму
            gameObject.AddComponent<PolygonCollider2D>(); // добавить новую по новому спрайту
        }
    }
}
