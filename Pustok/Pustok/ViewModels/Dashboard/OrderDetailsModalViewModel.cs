namespace Pustok.ViewModels.Dashboard;

public class OrderDetailsModalViewModel
{
    public OrderDetailsModalViewModel(string name, string category, string color, string size, int quantity, int totalProduct)
    {
        Name = name;
        Category = category;
        Color = color;
        Size = size;
        Quantity = quantity;
        TotalProduct = totalProduct;
    }

    public string Name { get; set; }
    public string Category { get; set; }
    public string Color { get; set; }
    public string Size { get; set; }
    public int Quantity { get; set; }
    public int TotalProduct {  get; set; }

}
