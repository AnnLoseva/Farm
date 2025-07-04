using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] public List<Item> slots = new List<Item>();
    [SerializeField] private int maxSlots = 30;

    [Header("Money")]
    [SerializeField] private int money = 100;

    [Header("Seeds")]
    [SerializeField] private List<SeedSlot> seedSlots = new List<SeedSlot>();
    [SerializeField] private int buySeedsAmount = 10;

    // Список на продажу
    private List<int> sellIndices = new List<int>();

    // Добавить предмет
    public void AddItem(Item item)
    {
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

    public void BuySeed(Seed seed)
    {
        var slot = seedSlots.Find(s => s.seed == seed);

        if (slot == null)
        {
            Debug.LogWarning("Семя не найдено в списке!");
            return;
        }

        if (money < seed.price)
        {
            Debug.LogWarning("Недостаточно денег!");
            return;
        }

        if (slot.amount + buySeedsAmount > slot.max)
        {
            Debug.LogWarning("Достигнут лимит семян!");
            return;
        }

        money -= seed.price;
        slot.amount += buySeedsAmount;

        Debug.Log($"Куплено {buySeedsAmount} семян {seed.seedName}. Теперь: {slot.amount} шт., денег осталось: {money}");

        OnInventoryChanged();
    }

    public List<Seed> GetAvailableSeeds()
    {
        List<Seed> result = new List<Seed>();
        foreach (var slot in seedSlots)
        {
            if (slot.amount > 0)
                result.Add(slot.seed);
        }
        return result;
    }

    public int GetSeedAmount(Seed seed)
    {
        SeedSlot slot = seedSlots.Find(s => s.seed == seed);
        return slot.amount;
    }

    public bool HasSeed(Seed seed, int amount = 1)
    {
        var slot = seedSlots.Find(s => s.seed == seed);
        return slot != null && slot.amount >= amount;
    }

    public bool UseSeed(Seed seed, int amount = 1)
    {
        var slot = seedSlots.Find(s => s.seed == seed);

        if (slot == null || slot.amount < amount)
        {
            Debug.LogWarning("Недостаточно семян!");
            return false;
        }

        slot.amount -= amount;
        Debug.Log($"Посажено {amount} семян {seed.seedName}. Осталось: {slot.amount}");
        OnInventoryChanged();
        return true;
    }

    public bool CheckRecipe(Recipe recipe)
    {
        foreach (var ing in recipe.ingredients)
        {
            if (!HasItem(ing.item, ing.amount))
                return false;
        }
        return true;
    }

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

    public bool RemoveItem(Item item, int count = 1)
    {
        if (!HasItem(item, count))
        {
            Debug.LogWarning($"Попытка убрать {count}×{item.itemName}, а есть только {slots.FindAll(s => s == item).Count}");
            return false;
        }

        int removed = 0;
        for (int i = slots.Count - 1; i >= 0 && removed < count; i--)
        {
            if (slots[i] == item)
            {
                slots.RemoveAt(i);
                removed++;
            }
        }

        OnInventoryChanged();
        Debug.Log($"Удалено {removed}×{item.itemName}, осталось {slots.FindAll(s => s == item).Count}");
        return true;
    }

    public int UseMoney(int amount)
    {
        money += amount;
        OnInventoryChanged();
        return money;
    }

    public int GetMoney()
    {
        return money;
    }

    // --- ПРОДАЖА ---

    public void ToggleSellItemAt(int index)
    {
        if (sellIndices.Contains(index))
            sellIndices.Remove(index);
        else
            sellIndices.Add(index);

        OnInventoryChanged();
    }

    public bool IsItemMarkedForSale(int index)
    {
        return sellIndices.Contains(index);
    }

    /// <summary>
    /// Продаёт все предметы в списке на продажу и очищает его.
    /// </summary>
    public void SellMarkedItems()
    {
        sellIndices.Sort();
        sellIndices.Reverse(); // Удаляем с конца, чтобы индексы не смещались

        foreach (int index in sellIndices)
        {
            if (index >= 0 && index < slots.Count)
            {
                Item item = slots[index];
                money += item.price;
                slots.RemoveAt(index);
            }
        }

        sellIndices.Clear();
        OnInventoryChanged();
    }

    private void Start()
    {
        // начальная инициализация при необходимости
    }

    public delegate void InventoryChanged();
    public event InventoryChanged OnInventoryChanged = delegate { };
}

