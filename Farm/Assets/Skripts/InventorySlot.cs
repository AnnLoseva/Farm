[System.Serializable]
public class InventorySlot
{
    public Item item;       // какой предмет
    public int amount;      // сколько штук

    public InventorySlot(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;
    }
}