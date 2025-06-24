using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    [SerializeField] public List<Item> slots = new List<Item>();
    [SerializeField] private int maxSlots = 30;    // максимальное число разных предметов
    [SerializeField] private int money = 100;
    [SerializeField] private List<Item> startItems;

    // Добавить предмет
    public void AddItem(Item item)
    {
        // иначе создаём новый, если есть место
        if (slots.Count < maxSlots)
        {
            slots.Add(item);

            Debug.Log("*************   Added : " + item.itemName + "************");

            OnInventoryChanged();

        }
        else
        {
            Debug.LogWarning("Инвентарь полон!");
        }
    }

    // Убрать предмет

    /// <summary>
    /// Проверить, есть ли в инвентаре хотя бы <paramref name="count"/> штук <paramref name="item"/>.
    /// </summary>
    public bool CheckRecipe(Recipe recipe)
    {
        foreach (var ing in recipe.ingredients)
        {
            if (!HasItem(ing.item, ing.amount))
                return false;
        }
        return true;
    }

    /// <summary>
    /// Проверить, есть ли в инвентаре хотя бы count штук item.
    /// </summary>
    public bool HasItem(Item item, int count = 1)
    {
        int found = 0;
        foreach (var slot in slots)
        {
            if (slot == item)
            {
                found++;
                if (found >= count)
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Удалить из инвентаря <paramref name="count"/> штук <paramref name="item"/>.
    /// Возвращает true, если удалено нужное количество, иначе false (и изменений не делает).
    /// </summary>
    public bool RemoveItem(Item item, int count = 1)
    {
        // сначала проверяем, что предметов достаточно
        if (!HasItem(item, count))
        {
            Debug.LogWarning($"Попытка убрать {count}×{item.itemName}, а есть только {slots.FindAll(s => s == item).Count}");
            return false;
        }

        // удаляем с конца (чтобы не смещать оставшиеся в начале)
        int removed = 0;
        for (int i = slots.Count - 1; i >= 0 && removed < count; i--)
        {
            if (slots[i] == item)
            {
                slots.RemoveAt(i);
                removed++;
            }
        }

        // уведомляем UI
        OnInventoryChanged();
        Debug.Log($"Удалено {removed}×{item.itemName}, осталось {slots.FindAll(s => s == item).Count}");
        return true;
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
            
            for(int i = 0; i < startItems.Count; i++)
            {
                AddItem(startItems[i]);
            }
            
            OnInventoryChanged();
        }
    }

    // Событие, если понадобится обновлять UI
    public delegate void InventoryChanged();
    public event InventoryChanged OnInventoryChanged = delegate { };
}
