namespace InventoryManagement.Application.Dtos.Products
{
    public class ProductDto
    {
        public int Id { get; set; }
        //[Required]
        public string? Name { get; set; }

        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
    }
}
