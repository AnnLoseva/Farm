using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();
    [SerializeField] private int maxSlots = 20;    // максимальное число разных предметов
    [SerializeField] private int money = 100;
    [SerializeField] private Item startItem;
    [SerializeField] private int itemCount;

    // Добавить предмет
    public void AddItem(Item item, int count = 1)
    {
        // ищем существующий слот
        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                slot.amount += count;
                OnInventoryChanged();
                Debug.Log("*************" + item.itemName + " : " +  slot.amount + "************");

                return;

            }
        }

        // иначе создаём новый, если есть место
        if (slots.Count < maxSlots)
        {
            slots.Add(new InventorySlot(item, count));

            Debug.Log("*************" + item.itemName + " : " + slots[slots.Count-1].amount + "************");

            OnInventoryChanged();

        }
        else
        {
            Debug.LogWarning("Инвентарь полон!");
        }
    }

    // Убрать предмет
    public bool RemoveItem(Item item, int count = 1)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = slots[i];
            if (slot.item == item)
            {
                if (slot.amount >= count)
                {
                    slot.amount -= count;
                    if (slot.amount == 0)
                        slots.RemoveAt(i);
                    OnInventoryChanged();

                    Debug.Log("*************" + item.itemName + " : " + slot.amount + "************");

                    return true;
                }
                break;
            }
        }

        Debug.LogWarning("Недостаточно предметов!");
        return false;
    }

    public bool HasIngredients(Recipe recipe)
    {
        foreach (var ing in recipe.ingredients)
            if (!HasItem(ing.item, ing.amount))
                return false;
        return true;
    }

    // Проверить, есть ли в инвентаре хотя бы count штук item
    public bool HasItem(Item item, int count = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item && slot.amount >= count)
                return true;
        }
        return false;
    }


    public int CheckMoney()
    {
        return money;
    }

    public void UseMoney(int amount)
    {
        money += amount;
    }

    private void Start()
    {
        if (slots.Count < maxSlots)
        {
            slots.Add(new InventorySlot(startItem ,itemCount));
            OnInventoryChanged();
        }
    }

    // Событие, если понадобится обновлять UI
    public delegate void InventoryChanged();
    public event InventoryChanged OnInventoryChanged = delegate { };
}
