using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField]private Transform itemsWindow;      // панель с GridLayoutGroup
    [SerializeField] private Inventory inventory;        // твой скрипт Inventory
    [SerializeField] private GameObject slotPrefab;      // префаб «Item Button»

    [Header("Money")]
    [SerializeField] private List<Text> moneyText;

    [Header("Settings")]
    [SerializeField] private int totalSlots = 30;        // всего слотов в окне

    private List<Button> slotButtons = new List<Button>();


    private void Awake()
    {

        // 1) Создаём нужное число слотов
        for (int i = 0; i < totalSlots; i++)
        {
            var go = Instantiate(slotPrefab, itemsWindow);
            go.active = true;
            var btn = go.GetComponent<Button>();
            slotButtons.Add(btn);
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
        foreach (var mText in moneyText)
        {

            mText.text = inventory.GetMoney().ToString() + "$";
        }

        //  Для каждого слота заполняем данные или очищаем
        for (int i = 0; i < slotButtons.Count; i++)
        {
            var currentButton = slotButtons[i];
            // Предполагаем, что внутри префаба:
            // — Image для иконки лежит в дочернем объекте "Image"
            // — Text для цены в дочернем объекте "Text (Legacy)"
            var iconImage = currentButton.transform.Find("Image").GetComponent<Image>();
            var priceText = currentButton.transform.Find("Text (Legacy)").GetComponent<Text>();

            if (i < inventory.slots.Count)
            {
                // есть предмет
                var slot = inventory.slots[i];
                iconImage.sprite = slot.icon;
                priceText.text = slot.price.ToString();
                currentButton.interactable = true;
            }
            else
            {
                // пустая ячейка
                iconImage.sprite = null;
                priceText.text = "";
                currentButton.interactable = false;
            }
        }


        // Money
        



    }
}
