using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Recipe")]
public class Recipe : ScriptableObject
{
    public string recipeName;

    [Tooltip("������ ����������� ������������")]
    public List<ItemAmount> ingredients;

    [Tooltip("��� ��������� � ����������")]
    public Item result;
    public int resultCount = 1;

    [Tooltip("ID ��������")]
    public int animationID = 0;
}

/// <summary>���� �������� + ����������</summary>
[System.Serializable]
public class ItemAmount
{
    public Item item;
    public int amount;
}
