namespace InventoryManagement.Web.ViewModels.Products
{
    public class EditProduct
    {
        public int Id { get; set; }
        //[Required]
        public string? Name { get; set; }

        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
    }
}
