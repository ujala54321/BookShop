using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShoppingCartMartUI.Controllers
{
    [Authorize]
    public class UserOrderController : Controller
    {
        private readonly IUserOrderRepository _userOrderRepo;

        public UserOrderController(IUserOrderRepository userOrderRepo)
        {
            _userOrderRepo = userOrderRepo;
        }
        public async Task<IActionResult> UserOrders()
        {
            var orders =await _userOrderRepo.GetUserOrders();

            return View(orders);
        }
    }
}
