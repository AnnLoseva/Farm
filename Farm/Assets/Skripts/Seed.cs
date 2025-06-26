using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Seeds")]
public class Seed : ScriptableObject
{
    public string seedName;
    public Sprite icon;
    public Item cropResult;   // ��� ������ ����� �����
    public int yieldCount = 1;
    public int growthAnimationID = 0;
    public int price;
}