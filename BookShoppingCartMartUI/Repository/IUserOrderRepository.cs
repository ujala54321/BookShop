namespace BookShoppingCartMartUI.Repository
{
    public interface IUserOrderRepository
    {
        Task<IEnumerable<Order>> GetUserOrders(bool getAll=false);
        Task ChangeOrderStatus(UpdateOrderStatusModel data);
        Task TogglePaymentStatus(int orderId);
        Task<Order> GetOrderById(int id);
        Task<IEnumerable<OrderStatus>> GetOrderStatuses();
    }
}