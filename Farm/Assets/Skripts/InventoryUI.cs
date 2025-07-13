using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField]private Transform itemsWindow;      // панель с GridLayoutGroup
    [SerializeField] private Inventory inventory;        // твой скрипт Inventory
    [SerializeField] private GameObject slotPrefab;      // префаб «Item Button»
    [SerializeField] private Text sellListText; // или TMP_Text, если используешь TextMeshPro


    [Header("Money")]
    [SerializeField] private List<Text> moneyText;

    [Header("Settings")]
    [SerializeField] private int totalSlots = 30;        // всего слотов в окне
    [SerializeField] private int shopSots = 8;

    private List<Button> itemsSlotButtons = new List<Button>();
    private List<Button> shopSlotButtons = new List<Button>();



    private void Awake()
    {

        // 1) Создаём нужное число слотов
        for (int i = 0; i < totalSlots; i++)
        {
            var slot = Instantiate(slotPrefab, itemsWindow);
            slot.active = true;
            var btn = slot.GetComponent<Button>();
            itemsSlotButtons.Add(btn);

        }


    }

    private void OnEnable()
    {
        // Подписываемся на событие изменения инвентаря
        inventory.OnInventoryChanged += RefreshUI;
        RefreshUI();
    }

    private void OnDisable()
    {
        inventory.OnInventoryChanged -= RefreshUI;
    }


    private void RefreshUI()
    {
        for (int i = 0; i < itemsSlotButtons.Count; i++)
        {
            var currentButton = itemsSlotButtons[i];
            var iconImage = currentButton.transform.Find("Image").GetComponent<Image>();
            var priceText = currentButton.transform.Find("Text (Legacy)").GetComponent<Text>();


            foreach (Text mText in moneyText)
            {

                mText.text = inventory.GetMoney().ToString() + "$";
            }

            if (i < inventory.slots.Count)
            {
                var item = inventory.slots[i];

                iconImage.enabled = true;
                iconImage.sprite = item.icon;
                priceText.text = item.price.ToString() + "$";
                currentButton.interactable = true;

                // Захватываем индекс
                int index = i;

                currentButton.onClick.RemoveAllListeners();
                currentButton.onClick.AddListener(() =>
                {
                    inventory.ToggleSellItemAt(index);
                    RefreshUI();
                });

                currentButton.image.color = inventory.IsItemMarkedForSale(index) ? Color.yellow : Color.white;
            }
            else
            {
                iconImage.enabled = false;
                priceText.text = "";
                currentButton.interactable = false;
                currentButton.onClick.RemoveAllListeners();
                currentButton.image.color = Color.white;
            }

            if (sellListText != null)
            {
                List<Item> sellList = inventory.GetSellList();
                if (sellList.Count == 0)
                {
                    sellListText.text = "";
                }
                else
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (var item in sellList)
                    {
                        sb.AppendLine($"{item.itemName}    {item.price}$");
                    }
                    sellListText.text = sb.ToString();
                }
            }
        }
    }


        // Money
        



    }

