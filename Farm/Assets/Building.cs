using System.Data.SqlTypes;
using System.Diagnostics;
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
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = emptySprite;
    }

    private void Update()
    {
        if (isBuilding)
        {
            currentBuildingTime += Time.deltaTime;
            if(currentBuildingTime > buildTime)
            {
                isBuilding = false;
                currentBuildingTime = 0;
                isBuild = true;
                spriteRenderer.sprite = readySprite;
            }
        }
    }

    public int Click(int money)
    {

        if (!isBuild & !isBuilding)
        {
            Build(money);

        }

        return money;
    }

    private int Build(int money)
    {
        if (money >= price)
        {
            money -= price;
            spriteRenderer.sprite = buildingSprite;
            isBuilding = true;
            Debug.Log(money);

        }

        return money;

    }
}
