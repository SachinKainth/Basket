namespace Domain.Entity
{
    public class BulkBuyOffer
    {
        public int ProductId { get; set; }
        public int NumberToBuy { get; set; }
        public int NumberFree { get; set; }

        public override bool Equals(object obj)
        {
            var bulkBuyOffer = obj as BulkBuyOffer;

            if (bulkBuyOffer == null)
            {
                return false;
            }

            return 
                (ProductId == bulkBuyOffer.ProductId) && 
                (NumberToBuy == bulkBuyOffer.NumberToBuy) &&
                (NumberFree == bulkBuyOffer.NumberFree);
        }

        // TODO Override GetHashCode
    }
}