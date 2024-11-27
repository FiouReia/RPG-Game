namespace Engine
{
    public class QuestReward
    {

        public Item Details { get; set; }
        public int Quantity { get; set; }
        public QuestReward(Item details, int quantity)
        {
            Details = details;
            Quantity = quantity;
        }

    }
}
