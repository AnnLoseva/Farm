using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;    // уникальное имя
    public Sprite icon;        // иконка для UI
    // при желании можно добавить: описание, цена, вес и т.п.
}
