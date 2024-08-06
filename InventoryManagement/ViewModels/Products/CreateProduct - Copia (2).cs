namespace InventoryManagement.Web.ViewModels.Products
{
    public class DProduct
    {
        public int ProductId { get; set; }
        //[Required]
        public string? Name { get; set; }

        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
    }
}
