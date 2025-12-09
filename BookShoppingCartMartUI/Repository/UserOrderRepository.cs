
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BookShoppingCartMartUI.Repository
{
    public class UserOrderRepository : IUserOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        public UserOrderRepository(ApplicationDbContext db,
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task ChangeOrderStatus(UpdateOrderStatusModel data)
        {
            var order = await _db.Orders.FindAsync(data.OrderId);
            if(order == null)
            {
                throw new InvalidOperationException($"order withi id:{data.OrderId} does not found");

            }
            order.OrderStatusId = data.OrderStatusId;
            await _db.SaveChangesAsync();

        }

        public async Task<Order> GetOrderById(int id)
        {
            return await _db.Orders.FirstAsync();
        }

        public async Task<IEnumerable<OrderStatus>> GetOrderStatuses()
        {
            return await _db.OrderStatuses.ToListAsync();
        }

        public async Task TogglePaymentStatus(int orderId)
        {
            var order = await _db.Orders.FindAsync(orderId);
            if (order == null)
            {
                throw new InvalidOperationException($"order withi id:{orderId} does not found");

            }
            order.IsPaid = !order.IsPaid;
            await _db.SaveChangesAsync();
        }

       /* public async Task<IEnumerable<Order>> UserOrders()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            var orders =await _db.Orders
                .Include(x => x.OrderStatus)
                   .Include(x => x.OrderDetail)
                   .ThenInclude(x => x.Book)
                   .ThenInclude(x => x.Genre)
                   .Where(a => a.UserId == userId)
                   .ToListAsync();
            return orders;

        */

        public async Task<IEnumerable<Order>> GetUserOrders(bool getAll = false)
        {
            IQueryable<Order> orders;
            var baseQuery = _db.Orders
                .Include(x => x.OrderStatus)
                .Include(x => x.OrderDetail)
                .ThenInclude(x => x.Book)
                .ThenInclude(x => x.Genre)
                .AsQueryable();
            if (getAll)
            {
                orders = baseQuery;
                
            }
            else
            {
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    throw new Exception("User is not logged-in");
                }
                orders = baseQuery.Where(a => a.UserId == userId);
                return await orders.ToListAsync();
            }
            return await orders.ToListAsync();

        }

        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }

   
}
