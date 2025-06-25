using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] public List<Item> slots = new List<Item>();
    [SerializeField] private int maxSlots = 30;    // максимальное число разных предметов


    [Header("Money")]
    [SerializeField] private int money = 100;



    [Header("Seeds")]
    [SerializeField] private int maxSeeds = 99;
    [SerializeField] private int buySeedsAmount = 10;

    [Header("Corn")]
    [SerializeField] private int cornSeedsAmount;
    [SerializeField] private int cornSeedsPrice = 10;

    [Header("Potato")]
    [SerializeField] private int potatoSeedsAmount;
    [SerializeField] private int potatoSeedsPrice = 10;

    [Header("Beet")]
    [SerializeField] private int beetSeedsAmount;
    [SerializeField] private int beetSeedsPrice = 10;

    [Header("Wheat")]
    [SerializeField] private int wheatSeedsAmount;
    [SerializeField] private int wheatSeedsPrice = 10;



    public enum SeedType
    {
        Corn,
        Potato,
        Beet,
        Wheat
    }


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

    /// <summary>
    /// Покупает одно семя указанного типа, если хватает денег и места.
    /// </summary>
    public void BuySeed(SeedType type)
    {
        int price = GetSeedPrice(type);
        int current = GetSeedAmount(type);

        if (money < price)
        {
            Debug.LogWarning("Недостаточно денег!");
            return;
        }
        if (current >= maxSeeds)
        {
            Debug.LogWarning("Достигнут лимит семян этого типа!");
            return;
        }

        // отнимаем деньги и прибавляем семя
        money -= price;
        SetSeedAmount(type, current + buySeedsAmount);

        Debug.Log($"Куплено 10 семян {type}. Теперь: {GetSeedAmount(type)} шт., денег осталось: {money}");

        OnInventoryChanged();
    }

    public void BuySeedByIndex(int typeIndex)
    {
        BuySeed((SeedType)typeIndex);
    }

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


    /// <summary>
    /// Использует деньги  <paramref name="money"/> += <paramref name="amount"/>
    /// </summary>
    public int UseMoney(int amount)
    {
        money += amount;

        OnInventoryChanged();

        return money;

    }

   
    public int GetSeedPrice(SeedType type)
    {
        switch (type)
        {
            case SeedType.Corn: return cornSeedsPrice;
            case SeedType.Potato: return potatoSeedsPrice;
            case SeedType.Beet: return beetSeedsPrice;
            case SeedType.Wheat: return wheatSeedsPrice;
            default: return 0;
        }
    }

    public int GetSeedAmount(SeedType type)
    {
        switch (type)
        {
            case SeedType.Corn: return cornSeedsAmount;
            case SeedType.Potato: return potatoSeedsAmount;
            case SeedType.Beet: return beetSeedsAmount;
            case SeedType.Wheat: return wheatSeedsAmount;
            default: return 0;
        }
    }

    public void SetSeedAmount(SeedType type, int value)
    {
        switch (type)
        {
            case SeedType.Corn:
                cornSeedsAmount = value;
                break;
            case SeedType.Potato:
                potatoSeedsAmount = value;
                break;
            case SeedType.Beet:
                beetSeedsAmount = value;
                break;
            case SeedType.Wheat:
                wheatSeedsAmount = value;
                break;
        }
    }

    public void UseSeed(SeedType type)
    {
        int current = GetSeedAmount(type);

        if (current <= 0)
        {
            Debug.LogWarning("Недостаточно семян!");
            return;
        }
       
        SetSeedAmount(type, current -1);

        Debug.Log($"Посажено 1 семя {type}. Теперь: {GetSeedAmount(type)} шт., Семян осталось: {current}");
    }

    private void Start()
    {
     
    }

    // Событие, если понадобится обновлять UI
    public delegate void InventoryChanged();
    public event InventoryChanged OnInventoryChanged = delegate { };
}
