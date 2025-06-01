using UnityEngine;
using UnityEngine.UI;

public class Controler : MonoBehaviour
{
    [SerializeField] private int money = 10;
    [SerializeField] private Text moneyText;

    private void Start()
    {
        moneyText.text = money.ToString();
    }

    void Update()
    {
        // Мышь (ПК)
        if (Input.GetMouseButtonDown(0))
        {
            DetectClick(Input.mousePosition);
        }

        // Тап (мобилка)
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            DetectClick(Input.GetTouch(0).position);
        }
    }

    void DetectClick(Vector2 screenPosition)
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(screenPosition);

        RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);

        if (hit.collider != null)
        {

            if (hit.collider.CompareTag("Building"))
            {
                Building building = hit.collider.GetComponent<Building>();
                money = building.Click(money);
                moneyText.text = money.ToString();
            }
                
        }
    }
}

