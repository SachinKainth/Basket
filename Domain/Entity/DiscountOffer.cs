namespace Domain.Entity
{
    public class DiscountOffer
    {
        public int ProductBoughtId { get; set; }
        public int NumberToBuy { get; set; }
        public int ProductDiscountedId { get; set; }
        public decimal PercentageOff { get; set; }

        public override bool Equals(object obj)
        {
            var discountOffer = obj as DiscountOffer;

            if (discountOffer == null)
            {
                return false;
            }

            return
                (ProductBoughtId == discountOffer.ProductBoughtId) &&
                (NumberToBuy == discountOffer.NumberToBuy) &&
                (ProductDiscountedId == discountOffer.ProductDiscountedId) &&
                (PercentageOff == discountOffer.PercentageOff);
        }

        // TODO Override GetHashCode
    }
}
