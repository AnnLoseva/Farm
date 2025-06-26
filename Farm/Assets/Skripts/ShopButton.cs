using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [Header("UI Elements")]
    private Image iconImage;
    private Text priceText; // Заменить на TMP_Text если нужно
    private Button button;

    [Header("Seed Info")]
    [SerializeField] private Seed seed;

    private Inventory inventory;

    void Start()
    {
        inventory = FindAnyObjectByType<Inventory>();

        if (seed != null)
            Setup(seed);
    }

    public void Setup(Seed newSeed)
    {
        seed = newSeed;

        Image image = GetComponent<Image>();
        iconImage = transform.Find("Icon").GetComponent<Image>();
        iconImage.sprite = seed.icon;

        priceText = GetComponentInChildren<Text>();
            priceText.text = $"{seed.price}$"; // предполагается, что buyPrice есть у семени

        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(BuySeed);
        
    }

    private void BuySeed()
    {
        if (seed != null && inventory != null)
        {
            inventory.BuySeed(seed);
        }
    }
}