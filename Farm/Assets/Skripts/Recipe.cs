using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName;

    [Tooltip("Список необходимых ингредиентов")]
    public List<ItemAmount> ingredients;

    [Tooltip("Что получится в результате")]
    public Item result;
    public int resultCount = 1;

    [Tooltip("ID анимации")]
    public int animationID = 0;
}

/// <summary>Пара «предмет + количество»</summary>
[System.Serializable]
public class ItemAmount
{
    public Item item;
    public int amount;
}
