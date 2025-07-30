public struct InventoryContent
{
    public int amount;
    public FuckingItemDataBaby data;

    public InventoryContent(FuckingItemDataBaby itemData, int currentAmount)
    {
        this.amount = currentAmount;
        this.data = itemData;
    }
}