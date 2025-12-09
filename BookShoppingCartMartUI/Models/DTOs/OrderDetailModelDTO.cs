namespace BookShoppingCartMartUI.Models.DTOs;

public class OrderDetailModelDTO
{
    public int DivId { get; set; }
    public IEnumerable<OrderDetail> OrderDetail { get; set; }
}
