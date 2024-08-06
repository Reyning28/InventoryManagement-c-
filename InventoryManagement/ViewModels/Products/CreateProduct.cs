namespace InventoryManagement.Web.ViewModels.Products
{
    public class CreateProduct
    {
        //[Required]
        public string? Name { get; set; }

        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
    }
}
