namespace LinqRetrievalLab.Models.DTO
{
    public class ProductDTO
    {
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public int CategoryId { get; set; }
    }
}
