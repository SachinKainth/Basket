namespace Domain.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public bool Processed { get; set; }

        public override bool Equals(object obj)
        {
            var product = obj as Product;

            if (product == null)
            {
                return false;
            }

            return
                (Id == product.Id) &&
                (Name == product.Name) &&
                (UnitPrice == product.UnitPrice);
        }

        public Product ShallowCopy()
        {
            return (Product) MemberwiseClone();
        }

        // TODO Override GetHashCode
    }
}