public struct InventoryContent
{
    public FuckingItemDataBaby data;
    public int amount;

    public InventoryContent(FuckingItemDataBaby itemData, int currentAmount)
    {
        this.data = itemData;
        this.amount = currentAmount;
    }
}