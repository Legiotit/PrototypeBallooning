public class Item
{
    public ItemType itemType;
    public float positionX;
    public float positionY;

    public Item(ItemType itemType, float positionX, float positionY)
    {
        this.itemType = itemType;
        this.positionX = positionX;
        this.positionY = positionY;
    }
}

public enum ItemType
{
    Positive,
    Negative,
    Mine
}
